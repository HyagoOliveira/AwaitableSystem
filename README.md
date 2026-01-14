# Awaitable System

* Utility code for Awaitable
* Unity minimum version: **6000.1**
* Current version: **1.3.0**
* License: **MIT**

## How To Use

Just import the `ActionCode.AwaitableSystem` namespace and use any function from [AwaitableUtility](/Runtime/AwaitableUtility.cs) static class.

### Wait Functions

Similar to `WaitUntil` and `WaitWhile` coroutines, you can use `WaitUntilAsync()` and `WaitWhileAsync` functions:

```csharp
using UnityEngine;
using ActionCode.AwaitableSystem;

namespace YourCompany.YourGame.YourSystem
{
    [DisallowMultipleComponent]
    public sealed class Tester : MonoBehaviour
    {
        public bool IsReady { get; private set; }
        public bool IsLoading { get; private set; }

        private async void LoadAsync()
        {
            Debug.Log("Start to load...");
            
            await AwaitableUtility.WaitUntilAsync(() => IsReady);
            await AwaitableUtility.WaitWhileAsync(() => IsLoading);
            
            Debug.Log("Loading is done!");
        }
    }
}
```

Also, there is the `AwaitableUtility.WaitForSecondsRealtimeAsync()`.

### Lerp Functions

You can asynchronously linear interpolate two numbers: 

```csharp
using UnityEngine;
using ActionCode.AwaitableSystem;

namespace YourCompany.YourGame.YourSystem
{
    [DisallowMultipleComponent]
    public sealed class Tester : MonoBehaviour
    {
        private async void LerpAsync()
        {
            Debug.Log("Start to lerp...");

            // Linear interpolate between 1 and 8 in 4 seconds, using 1 as speed.
            await AwaitableUtility.LerpAsync(
                start: 1f,
                final: 8f,
                duration: 4f,
                ShowLerpValue,
                speed: 1f
            );
            
            Debug.Log("Lerp is done!");
        }

		// Your game code should use this value to do something.
        private static void ShowLerpValue(float value) => Debug.Log($"Lerp Value: {value}");
    }
}
```

Similarly, you can also Lerp colors using the overridden `LerpAsync(Color)` function.

If you want to Lerp other values type, use the generic `AwaitableUtility.InterpolateAsync<T>()` function, providing the get and set value callbacks.

> LerpAsyn functions use Unscaled Delta Time. Therefore, those functions will work when `Time.deltaTime == 0f` (the game is paused).

### MonoBehaviour Extension Functions

Inside MonoBehavior components, safelly wait for seconds asynchronously without throwing any exception if the component (or the GameObject itself) is destroyed during the waiting operation

```csharp
using ActionCode.AwaitableSystem;

public async void UnequipWeapon()
{
    // Play Unequip animation
    await this.WaitForSecondsAsync(2);
    // Dispose remain weapon
}
```

## Installation

### Using the Package Registry Server

Follow the instructions inside [here](https://cutt.ly/ukvj1c8) and the package **ActionCode-AwaitableSystem** 
will be available for you to install using the **Package Manager** windows.

### Using the Git URL

You will need a **Git client** installed on your computer with the Path variable already set. 

- Use the **Package Manager** "Add package from git URL..." feature and paste this URL: `https://github.com/HyagoOliveira/AwaitableSystem.git`

- You can also manually modify you `Packages/manifest.json` file and add this line inside `dependencies` attribute: 

```json
"com.actioncode.awaitable-system":"https://github.com/HyagoOliveira/AwaitableSystem.git"
```

---

**Hyago Oliveira**

[GitHub](https://github.com/HyagoOliveira) -
[BitBucket](https://bitbucket.org/HyagoGow/) -
[LinkedIn](https://www.linkedin.com/in/hyago-oliveira/) -
<hyagogow@gmail.com>