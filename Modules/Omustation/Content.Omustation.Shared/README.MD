# Dependency info

`Core` = `Content.Shared, Content.Server, Content.Client` <br>
`Modules` = `Content.<modulename>.Shared, Content.<modulename>.Server, Content.<modulename>.Client, (...)` <br>

Shared modules depend on `Content.Shared`, and like it, `Server` and `Client` modules will depend on it.

In practice, this means that since `Content.Shared` builds before `Content.Omustation.Shared` and its dependents, naturally, it has no knowledge of them.

In cases where we may need to communicate with code that exists in any dependents, we use `Content.Common` as a communicator module.
Since `Content.Shared` itself depends on it, we have a module which all parties can see

```
                         Content.Common
                               ┃
                               ┃
                               ┃
                         Content.Shared ━━━━━> (Content.Client, Content.Server, etc)
                               ┃
                               ┃
                               ┃
                    Content.Omustation.Shared
                               ┃
                 ┏━━━━━━━━━━━━━┻━━━━━━━━━━━━━┓
     Content.Omustation.Server  Content.Omustation.Client
```

If you need to reach a module from `Core`, you can find more info in the `Content.Common` module.
