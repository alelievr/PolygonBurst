# PolygonBurst, polygon shooter

Little 2D unity project with a simple procedural level generation.

## Levels
Levels are procedurally generated using an hilbert curve and a random square mask to get a random path.  
Then a naive polygonizer generate the interior of the level (much easier than generate the exterior).  
For colliders, i used 2D edge collider generated on the borderline of the level.
Here is it looks once generated:
![](https://image.noelshack.com/fichiers/2017/38/3/1505859018-screen-shot-2017-09-19-at-10-45-34-pm.png)
![](https://image.noelshack.com/fichiers/2017/38/3/1505859026-screen-shot-2017-09-19-at-10-45-16-pm.png)

## Attack patterns
To accelerate the attack pattern creation, i made a custom editor with a visualization of the emitter.  
You can ajust from here speed, color, size, rotation, direction and more.
Spawn inside circle, equal disposition.
![](https://image.noelshack.com/fichiers/2017/38/3/1505859033-screen-shot-2017-09-19-at-10-40-25-pm.png)
Custom points, random rotation
![](https://image.noelshack.com/fichiers/2017/38/3/1505859051-screen-shot-2017-09-19-at-10-40-55-pm.png)
Spawn on circle, equal disposition
![](https://image.noelshack.com/fichiers/2017/38/3/1505859054-screen-shot-2017-09-19-at-10-41-14-pm.png)

## Bosses
Bosses contains multiple patterns which can be activated by default, when their life fall under a limit or at their death.  
Here again i made a simple custom editor to simplify the creation of bosses:
![](https://image.noelshack.com/fichiers/2017/38/3/1505859475-screen-shot-2017-09-20-at-12-17-06-am.png)

## Post processing

I used the excellent unity's [post processing stack](https://www.assetstore.unity3d.com/en/#!/content/83912) asset for all my effects.  
Bloom, anti-aliasing, a bit of motion blur and chromatic aberration make the polygons more visible and the game much more beautiful.

## Game screens:
Boss 9:
![](https://image.noelshack.com/fichiers/2017/38/3/1505859515-screen-shot-2017-09-19-at-10-41-56-pm.png)
Boss WTF:
![](https://image.noelshack.com/fichiers/2017/38/3/1505859588-screen-shot-2017-09-19-at-10-43-05-pm.png)
Boss test:
![](https://image.noelshack.com/fichiers/2017/38/3/1505859616-screen-shot-2017-09-19-at-10-44-04-pm.png)
