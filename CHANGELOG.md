# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

## [1.4.0](https://github.com/Utconnect/utconnect-dotnet/compare/v1.3.0...v1.4.0) (2024-08-03)


### Features

* **identity/user:** add user to roles ([9fb2f2b](https://github.com/Utconnect/utconnect-dotnet/commit/9fb2f2b202dac80c0f2ea322c30a22b3504ba5c7))
* **identity/user:** returns username and id when create user ([00e9f8e](https://github.com/Utconnect/utconnect-dotnet/commit/00e9f8e41b5a0cd495f3ff58fbed2b80bc2e7d1a))
* **identity:** create user ([55d3464](https://github.com/Utconnect/utconnect-dotnet/commit/55d34642a609801d9d7a012434792db8079aaa12))
* **identity:** pre-populate role `teacher` on startup ([f3348a2](https://github.com/Utconnect/utconnect-dotnet/commit/f3348a2cf47bdb79cf92b3a6ffc4a662d28fb871))


### Improvements

* use concrete classes of `Error` instead of `Error` ([c911d6e](https://github.com/Utconnect/utconnect-dotnet/commit/c911d6e881d16f83f34908fd22af94850c6ed908))

## [1.3.0](https://github.com/Utconnect/utconnect-dotnet/compare/v1.2.0...v1.3.0) (2024-07-30)


### Features

* **identity:** forgot password ([1d61271](https://github.com/Utconnect/utconnect-dotnet/commit/1d61271e39cbd40376662b4c83072c775575feb9))
* **identity:** redirect to home page if logged-in user try to access public-only pages ([b4ff2b3](https://github.com/Utconnect/utconnect-dotnet/commit/b4ff2b33b6fad58f1035849766a0e9bde85e2966))
* **identity:** reset password ([df6f294](https://github.com/Utconnect/utconnect-dotnet/commit/df6f2948a7521016919d2cd3385fc8918c07587e))

## [1.2.0](https://github.com/Utconnect/utconnect-dotnet/compare/v1.1.3...v1.2.0) (2024-07-28)


### Features

* **identity/login:** get access token automatically when user is logged in ([8ad8a22](https://github.com/Utconnect/utconnect-dotnet/commit/8ad8a222bf3a14b3bba12b27d2a04e07dc33c2f0))

### [1.1.3](https://github.com/Utconnect/utconnect-dotnet/compare/v1.1.2...v1.1.3) (2024-07-25)


### Bug Fixes

* **identity:** dom elements should use the `autocomplete` attribute correctly ([0dd03a1](https://github.com/Utconnect/utconnect-dotnet/commit/0dd03a130bdbc28d5551d7b0b1a529cc6f3b7b33))
* **oidc:** specify the `RouteAttribute` ([10abb89](https://github.com/Utconnect/utconnect-dotnet/commit/10abb89913ff75e990c0460f2ca8cb3ac44e3f92))


### Improvements

* use `await RunAsync` instead of `Run` ([37d83b1](https://github.com/Utconnect/utconnect-dotnet/commit/37d83b104d0cab66d62ce3f81b2f9c12b7318522))

### [1.1.2](https://github.com/Utconnect/utconnect-dotnet/compare/v1.1.1...v1.1.2) (2024-07-22)


### CI/CD

* update sonar version when release ([0158805](https://github.com/Utconnect/utconnect-dotnet/commit/015880579f60222aae43a7c78c63bf63cbaeab42))

### [1.1.1](https://github.com/Utconnect/utconnect-dotnet/compare/v1.1.0...v1.1.1) (2024-07-17)


### Improvements

* use coffer to save database password ([f9e26ef](https://github.com/Utconnect/utconnect-dotnet/commit/f9e26efe9e28fced409159004abeb8454a2adb1e))

## [1.1.0](https://github.com/Utconnect/utconnect-dotnet/compare/v1.0.5...v1.1.0) (2024-07-16)


### Features

* use coffer to save jwt tokens ([51e4ae6](https://github.com/Utconnect/utconnect-dotnet/commit/51e4ae63b50cabdeb2de0a087abed3fe1f337285))

### [1.0.5](https://github.com/Utconnect/utconnect-dotnet/compare/v1.0.4...v1.0.5) (2024-07-14)


### Bug Fixes

* **shared:** `BaseEntity`'s ID should be typed `T` ([1a68578](https://github.com/Utconnect/utconnect-dotnet/commit/1a68578f2363e293cf7937aa6b2ad76da45b79b9))

### [1.0.4](https://github.com/Utconnect/utconnect-dotnet/compare/v1.0.3...v1.0.4) (2024-07-12)


### Bug Fixes

* **home:** add security flags when create cookie ([eec0d02](https://github.com/Utconnect/utconnect-dotnet/commit/eec0d02a7580bfbbf0151ad8e559f7c165d34bfd))
* **oidc:** cannot retrieve user info ([3c3bf74](https://github.com/Utconnect/utconnect-dotnet/commit/3c3bf747fbf51cada3a18fd07dbf693d85d02fc5))
* **oidc:** only display development exception page when environment is dev ([64e6e79](https://github.com/Utconnect/utconnect-dotnet/commit/64e6e79dd88152b517fb836f94cb0379e3ba02b5))


### CI/CD

* **release:** use latest action ([1fcf7d0](https://github.com/Utconnect/utconnect-dotnet/commit/1fcf7d006541e762f2dbe88571a8894e9caa9612))

### [1.0.3](https://github.com/Utconnect/utconnect-dotnet/compare/v1.0.2...v1.0.3) (2024-07-11)


### CI/CD

* build each project by dockerfile ([5adf030](https://github.com/Utconnect/utconnect-dotnet/commit/5adf030549ab4ba23c825eb86905a18b7f868b45))

### [1.0.2](https://github.com/Utconnect/utconnect-dotnet/compare/v1.0.1...v1.0.2) (2024-07-10)


### CI/CD

* **release:** build and push images to docker ([9e04de9](https://github.com/Utconnect/utconnect-dotnet/commit/9e04de9670ae3f769fc2f145d2af0f4f14326862))
* **release:** use node v20 ([1c7b08a](https://github.com/Utconnect/utconnect-dotnet/commit/1c7b08a0ef42500fba23b28594245744636052c9))
* update readme sections for auto-generate ([b994a26](https://github.com/Utconnect/utconnect-dotnet/commit/b994a26b3455e0801422b471c691f3cd0929d7e2))

### 1.0.1 (2024-07-10)

## 1.0.0 (2024-07-09)


### Features

* add base for project
