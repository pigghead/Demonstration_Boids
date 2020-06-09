# Boids
 Boids implemented using Character Controllers

Inspiration drawn from Sebastian Lague (https://www.youtube.com/watch?v=bqtqltqcQhw)

Boids (bird-like objects) are autonomous movers/agents that adhere to three basic rules: Cohesion, Avoidance, and Matching.

## Cohesion / Flocking
Boids will try to "group up." This is done by calculating the average position of each Boid relative to a single given Boid. Each boid will try to steer towards this calculated position.

In the image below, magenta colored rays are drawn from each Boid to the perceived "center of the flock," or simply the averages of all of their positions.

![CohesionImage](https://github.com/pigghead/Demonstration_Boids/blob/master/images/DOC_Cohesion.PNG)

## Avoidance
Boids will attempt not to collide with one another, as well as with obstacles and the walls. This is done by calculating the distance between any given Boid and other Boids, squaring the magnitude of the distance, and testing it against a value (in WanderScene, this is 0.7). The same formula is followed for obstacle avoidance.

In the image below, bright green rays are drawn from each Boid to every other Boid. There are also red rays drawn denoting the direction in which an agent is moving.

![BoidAvoidanceImage](https://github.com/pigghead/Demonstration_Boids/blob/master/images/DOC_FuturePos_BoidAvoidance.PNG)

The same is done for Obstacles. A ray is drawn from each Boid to each obstacle present in the map. The number of obstacles genereated is random between four and twelve. Those generated are done so at random positions within the plane.

![ObstacleAvoidanceImage](https://github.com/pigghead/Demonstration_Boids/blob/master/images/DOC_FuturePos_ObstacleAvoidance.PNG)

## Matching
Boids will attempt to match the speed of neighboring Boids. This will be accomplished much like the Cohesion / Flocking algorithm, except that the velocities of all Boids will be used as opposed to their positions.
