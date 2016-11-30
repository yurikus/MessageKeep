# MessageKeep

This application implements a hypothetical cloud user/message/channel management service. 

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

    GET      /users                                    list of users
    GET      /users/<user>                             summary of user's subbed channels and dm
    GET      /users/<user>/messages                    user's messages (direct or broadcast)
    GET      /users/<user>/messages/to/<user2>         list of user messages to user2
    POST     /users/<user>/messages/to/<user2>         post message to user2
    PUT      /users/<user>/channel/<chan>              subscribe user to channel
    DELETE   /users/<user>/channel/<chan>              unsubscibe user from channel
    POST     /users/<user>/channel/<chan>              post a message to channel
                                                        
    GET      /channels                                 list of channels
    GET      /channels/<chan>/users                    summary of subbed users and message count
    GET      /channels/<chan>/messages                 broadcast messages for the channel

## Content types

Input content type is expected to be and output content types is generated as `application/json`, hence when using `curl` a `content-type: application/json` header would be necessary.

## Notes

Miscellaneous notes are sprinkled in code where I thought a clarification would be of benefit. Otherwise I think code is fairly self-describing.