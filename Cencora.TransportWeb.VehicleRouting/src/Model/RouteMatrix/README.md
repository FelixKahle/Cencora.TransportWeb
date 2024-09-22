# Route Matrix
## Overview

The Directed Route Matrix is a fundamental component in the Vehicle Routing Problem, used to efficiently store and retrieve travel times and distances between various locations represented as nodes in a graph. This matrix serves as a critical tool for determining the best routes for vehicles, as it allows for the rapid lookup of the time or distance required to travel from one node to another. By precomputing and organizing this data into a matrix format, the routing algorithm can quickly assess potential routes, optimize paths, and ensure that vehicles follow the most efficient paths possible.

### Design
The Directed Route Matrix is designed with flexibility and efficiency in mind, and part of this design is its inheritance of the `IReadOnlyDirectedRouteMatrix` and `IDirectedRouteMatrix` interface. This interfaces define a set of functionalities that the Directed Route Matrix must implement, ensuring that the matrix can be used in a consistent and predictable manner within the broader routing framework.

## Edges in the Route Matrix

In the context of the Directed Route Matrix, edges represent the possible paths or connections between nodes, with each edge carrying specific information about the travel time and distance between two points. These edges can be classified into two distinct types, each serving a different purpose in the routing process:

- **DefinedRouteEdge**: A `DefinedRouteEdge` represents a valid and feasible route from one node to another within the matrix. It indicates that there is a direct path between the two nodes, along which a vehicle can travel. The DefinedRouteEdge stores key information such as the travel time and distance associated with this route, allowing the routing algorithm to make informed decisions about the most efficient paths. The direction of travel is crucial in this context, as it defines the one-way nature of the route, which can differ in time or distance depending on the direction.
- **UndefinedRouteEdge**: An `UndefinedRouteEdge` signifies that it is impossible to travel directly from one node to another in the specified direction. This might occur due to physical barriers, regulatory restrictions, or other constraints that prevent a direct route from being established between the two points. The presence of an UndefinedRouteEdge in the matrix indicates that alternative routes or nodes must be considered to complete the journey, as the direct connection is not feasible. The directionality of the edge is important, as it might be possible to travel between the two nodes in the opposite direction, even if the direct route in the specified direction is unavailable.

## Read Only
### `IReadOnlyDirectedRouteMatrix` Interface

The `IReadOnlyDirectedRouteMatrix` interface provides a blueprint for the Directed Route Matrix, focusing on read-only access to the travel times and distances between nodes. By inheriting this interface, the Directed Route Matrix is required to implement certain methods and properties that allow it to be queried efficiently without allowing modification of the underlying data. This ensures that the matrix remains consistent and reliable throughout the routing process, preventing accidental changes that could disrupt route calculations.

## Conclusion

The Directed Route Matrix and its associated edges are essential components in modeling and solving the Vehicle Routing Problem. By clearly defining the relationships between nodes through `DefinedRouteEdges` and `UndefinedRouteEdges`, the matrix provides a comprehensive view of the possible routes, enabling the optimization of vehicle paths for efficiency and cost-effectiveness. The matrix structure ensures that travel times and distances are readily accessible, supporting real-time decision-making and enhancing the overall performance of the routing algorithm.