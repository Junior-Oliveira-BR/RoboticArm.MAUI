
# RoboticArm

![Status](http://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=RED&style=for-the-badge)


## Introduction

This project uses .NET MAUI and Evergine to control a 5-axis robotic arm. It was developed with the goal of exploring the integration between these two technologies, leveraging the cross-platform user interface capabilities of .NET MAUI and the powerful 3D rendering of Evergine. The project not only demonstrates the control of a robotic arm but also serves as a case study on how to combine a modern UI development platform with a robust graphics engine for interactive simulations and visualizations.

## Functionality
![](https://github.com/Junior-Oliveira-BR/RoboticArm.MAUI/blob/master/RoboticArm.MAUI/Resources/Assets/animation.gif)

The robotic arm can be moved along its independent axes, allowing precise control of each segment. Additionally, a basic inverse kinematics model is implemented to assist with positioning the arm, though this model is still a work in progress and requires further improvement.

### Collision Detection
We encountered issues with collision detection in Evergine. Currently, the collision detection feature is not functioning as expected.

```csharp
private bool DetectCollision()
{
    /* I couldn't get collisions to work correctly  (ಥ_ಥ)
     * Two problems occurring:
     *          False start collision
     *          End Collision is not called sometimes
     *          I must be missing something ¯\_(ツ)_/¯
     */

    foreach (var axis in DictEntities)
        if (axis.Value.IsColliding()) return true;

    return false;
}
```


## Installation
### Prerequisites
- .NET 6.0 or higher
- Visual Studio 2022 with MAUI support

### Installation Steps
1. Clone the repository:
    ```bash
    git clone https://github.com/Junior-Oliveira-BR/RoboticArm.MAUI.git
    cd RoboticArm.MAUI
    ```
2. Open the project in Visual Studio 2022.
3. Restore the dependencies:
    ```bash
    dotnet restore
    ```
4. Build and run the project.

## Contributing
We welcome contributions to the project! If you would like to contribute, you can refer to the [GitHub documentation on how to do so](https://docs.github.com/en/get-started/exploring-projects-on-github/contributing-to-a-project).

Thank you for your contributions!

## Contact
- **Email**: [junioroliveirabts@gmail.com](mailto:junioroliveirabts@gmail.com)
- **GitHub**: [Junior-Oliveira-BR](https://github.com/Junior-Oliveira-BR)
- **LinkedIn**: [José Donizete Oliveira Júnior](https://www.linkedin.com/in/josé-donizete-oliveira-júnior-65628348/)
