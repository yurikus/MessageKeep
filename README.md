# MessageKeep

This application implements a hypothetical cloud user/message/channel management service. It is self-hosting with Owin/Katana and uses WebAPI 2, NewtonSoft JSON and DryIoc DI container. Test project uses XUnit and VisualStudio runner. This is a VisualStudio 2015 solution.

## Running
Running without arguments will produce the following page of helpful text:  

    $ MessageKeep.exe
    ERROR(S):
      Required option 'port' is missing.
      --port       Required. Port to listen on.
      --public     Listen on all interfaces. Requires elevation or urlacl
                   registration. By default we listen on localhost.
      --help       Display this help screen.
      --version    Display version information.

Which hints at using `--port` argument to specify port to listen on: 

    $ MessageKeep.exe --port 8065
    Press Ctrl-Q to exit.
    Running with args: --port 8065
    Listening on http://localhost:8065

## Endpoints

Available endpoints, appropriate HTTP verbs and summary for each is presented below: 

    GET      /mk/users                                    list of users
    GET      /mk/users/<user>                             summary of user's channels
    GET      /mk/users/<user>/messages                    user's messages (direct or broadcast)
    GET      /mk/users/<user>/messages/since/<date>       user's messages (direct or broadcast) received on or after <date>
    GET      /mk/users/<user>/messages/to/<user2>         list of user messages to user2
    POST     /mk/users/<user>/messages/to/<user2>         post message to user2
    PUT      /mk/users/<user>/channel/<chan>              subscribe user to channel
    DELETE   /mk/users/<user>/channel/<chan>              unsubscibe user from channel
    POST     /mk/users/<user>/channel/<chan>              broadcast message to channel
                                                        
    GET      /mk/channels                                 list of channels
    GET      /mk/channels/<chan>/users                    summary of subbed users
    GET      /mk/channels/<chan>/messages                 list broadcast messages for the channel
    GET      /mk/channels/<chan>/messages/since/<date>    list broadcast messages for the channel posted on or after <date>

## Content types

Input content type is expected to be and output content types is generated as `application/json`, hence when using `curl` a `content-type: application/json` header would be necessary.

## Notes

- Miscellaneous notes are sprinkled in code where I thought a clarification would be of benefit. Otherwise I think code is fairly self-describing.
- Scripts in the `curl/` directory do a quick visual test of functionality. They expect app listening on port `8065` and a linux, LXSS, CYGWIN or MSYS environment as well as jq json pretty-printer (https://stedolan.github.io/jq/download/, use 32-bit version on x64 Windows). Binary is bundled in `curl/jq`.
- A proper production-ready app would also include: error/exception logging and recovery, configuration endpoints, output stream compression among other things.  