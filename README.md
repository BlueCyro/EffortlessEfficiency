# 👟💨 Effortless Efficiency: DIE DIE REFLECTION DIE

Effortless Efficiency is a mod for [Resonite](https://www.Resonite.com) via [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader) that aims to provide a continually-expanding patchset for increasing performance in certain key areas of the game to reduce microstutters and momentary hitching. More patches will be continually added as I discover areas that I can easily optimize.


# Why is this needed?

Resonite currently runs in Unity's crummy mono (version 5.11) runtime and so the quality of the codegen and the overall runtime performance are severely degraded from what modern .NET runtime versions can provide. This means that innocent uses of reflection and common programming patterns are likely not optimized very well when the code is executed.

Subsequently, you end up having to outsmart the runtime in some cases and essentially baby it into producing more optimal code output, or avoid those patterns altogether (e.g. caching reflection calls into runtime-generated delegates - much much faster)