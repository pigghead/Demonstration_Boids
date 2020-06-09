# Boids
 Boids implemented using Character Controllers

Inspiration drawn from Sebastian Lague (https://www.youtube.com/watch?v=bqtqltqcQhw)

Boids (bird-like objects) are autonomous movers/agents that adhere to three basic rules: Cohesion, Avoidance, and Matching.

## Cohesion / Flocking
Boids will try to "group up." This is done by calculating the average position of each Boid relative to a single given Boid. Each boid will try to steer towards this calculated position.

![CohesionImage](https://github.com/pigghead/Demonstration_Boids/blob/master/images/DOC_Cohesion.PNG)

## Avoidance
Boids will attempt not to collide with one another, as well as with obstacles and the walls. This is done by calculating the distance between any given Boid and other Boids, squaring the magnitude of the distance, and testing it against a value (in WanderScene, this is 0.7). The same formula is followed for obstacle avoidance.

## Matching
Boids will attempt to match the speed of neighboring Boids. [WORK IN PROGRESS]
