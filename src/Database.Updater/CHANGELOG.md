2.0.0
  - Allow injecting services into the `Migration` classes using the new `DatabaseUpdaterBuilder.ConfigureServices()` method.
  - Allow adding custom typed command line arguments and options using the new `DatabaseUpdaterBuilder.ConfigureCommandLine()` method,
    their values being retrievable in the `Migration` classes through the new `IDatabaseUpdaterCommandLine` service.

1.1.0
  - Internal improvements

1.0.0
  - Initial release.
