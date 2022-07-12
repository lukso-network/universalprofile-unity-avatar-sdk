# Universal Profile Avatar SDK
Universal Profile Avatar SDK for Unity. Used to load avatars from Universal Profiles on the [LUKSO Network](https://lukso.network/).

## Getting Started
To get started, clone this repository and import it into Unity. The project comes with [Nethereum](https://github.com/Nethereum/Nethereum/) dependencies that were only tested on **Unity 2020.3.30f1**.

Once imported you can use the `AvatarSDK` class from the `UniversalProfileSDK.Avatars` namespace to start loading profiles.

## Getting an UniversalProfile
The AvatarSDK API comes with the following methods.

- `GetProfileRemote(string address, GetProfileSuccess onSuccess, GetProfileFailed onFail)` - Loads a profile from the blockchain. 
- `GetProfileLocal(string filepath, GetProfileSuccess onSuccess, GetProfileFailed onFail)` - Loads a profile from the local file system.
- `LoadProfileFromJson(string json, GetProfileSuccess onSuccess, GetProfileFailed onFail)` - Loads a profile directly from a [LSP3Profile JSON](https://github.com/lukso-network/LIPs/blob/main/LSPs/LSP-3-UniversalProfile-Metadata.md#lsp3profile) string

These methods are meant to be used as coroutines with delegates passed to them as parameters on what to do once loading fails or succeeds.
- `GetProfileSuccess(UniversalProfile loadedProfile)` is a void delegate with the loaded `UniversalProfile` as it's parameter.
- `GetProfileFailed(Exception exception)` is a void delegate with the thrown `Exception` as it's parameter.

## Loading an Avatar
Once we have an `UniversalProfile` we can access it's `UPAvatar[]` property to get it's avatars.

To load a `UPAvatar` we can use the `AvatarCache` class from the `UniversalProfileSDK.Avatars` namespace using:
- `LoadAvatar(UPAvatar upAvatar, AvatarLoadSuccessDelegate onSuccess, AvatarLoadErrorDelegate onFailed, ProgressChangedDelegate onProgressChanged)` - Loads the avatar assetbundle that our `UPAvatar` points to.

If an asset bundle has been downloaded before it will be loaded from the cache, if not it will be downloaded first then cached.

As before, this is method is meant to run as a coroutine and calls these delegates in case of success or failure. 
- `AvatarLoadSuccessDelegate(GameObject avatar)` - If bundle download/load succeeds, the avatar is loaded and this delegate is called with the loaded avatar being it's parameter.
- `AvatarLoadErrorDelegate(Exception exception)` - If bundle download/load fails, this delegate is called with the exception that occured being it's parameter.
