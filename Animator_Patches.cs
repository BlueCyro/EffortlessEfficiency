using HarmonyLib;
using FrooxEngine;
using System.Linq.Expressions;

namespace EffortlessEfficiency;

public partial class EffortlessEfficiency
{
    [HarmonyPatch(typeof(Animator))]
    public class Animator_Patches
    {

        // This method that's being patched originally called Activator.CreateInstance each time it was invoked.
        // When you reflect a bunch of stuff in mono, it appears to gradually slow down other things that use reflection as well.
        // I have a feeling this is due to a big cache being built somewhere that is hit whenever reflection is used. This is
        // mostly exacerbated by CherryPick (https://github.com/BlueCyro/CherryPick), but could be triggered more broadly.
        // I hate Unity's mono runtime.
        [HarmonyPrefix]
        [HarmonyPatch("CreateFieldMapper")]
        public static bool CreateFieldMapper_Prefix(Type fieldType, ref object __result)
        {
            Debug($"Intercepting FieldMapper creation for type: {fieldType}");
            __result = EfficientCacheHelper.ConstructMapper(fieldType);
            return false;
        }
    }
}




public partial class EfficientCacheHelper
{

    // Cache mappers for quick access later
    static readonly Dictionary<Type, Func<object>> mappers = [];



    // Create constructor for a FieldMapper of a given type
    static Func<object> CreateConstructor(Type fieldType)
    {
        
        var constructor = typeof(Animator.FieldMapper<>).MakeGenericType(fieldType);

        var construct = Expression.New(constructor);
        var cast = Expression.TypeAs(construct, typeof(object));

        return Expression.Lambda<Func<object>>(cast).Compile();
    }



    /// <summary>
    /// Constructs an <see cref="Animator.FieldMapper{T}"/> of the given field type. Caches the constructor of each generic type for efficiency
    /// </summary>
    /// <param name="fieldType">The generic type parameter of the FieldMapper</param>
    /// <returns>The <see cref="Animator.FieldMapper{T}"/> in object form</returns>
    public static object ConstructMapper(Type fieldType)
    {
        if (mappers.TryGetValue(fieldType, out Func<object> constructor))
        {
            EffortlessEfficiency.Debug($"Cache HIT for FieldMapper of type: {fieldType}");
            return constructor();
        }
        else
        {
            EffortlessEfficiency.Debug($"Cache MISS for FieldMapper of type: {fieldType}");
            mappers.Add(fieldType, CreateConstructor(fieldType));
            return Activator.CreateInstance(typeof(Animator.FieldMapper<>).MakeGenericType(fieldType));
        }
    }
}