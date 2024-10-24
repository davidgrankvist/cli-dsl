# Syntax Ideas

## About

Here's a rough draft of the language idea. Enjoy.

## Sketch

```
# cli.cmd

cmd hello sh {
    echo hello
}

cmd goodbye ps {
    Write-Output hello
}

cmd all cmd {
    hello
    goodbye
}

cmd nested cmds {
    cmd thing sh {
        echo thing
    }

    cmd other sh {
        echo other
    }
}
```

Example:

```
$ cli.cmd nested thing
$ other
```

Env variables and args are passed down. What about help text?
What about executing the first part of a nested command?
What about defining args?

```
cmd nested cmds {
    self sh {
        echo "my own command without nesting"
    }

    cmd other sh {
        echo "this part is actually nested"
    }

    summary {
        This command does something.
    }

    arg help {
        Outputs the help.
    }

    arg verbose {
        Makes the thing verbose.
    }
}
```

Example:

```
$ cli.cmd nested --help
$ This command does something.
$
$ Parameters:
$ --help Outputs the help
$ --verbose Makes the thing verbose
```

What about passing down args?

```
cmd passer cmds {
    cmd self sh {
        echo $message
    }

    arg message {
        It's a message. Sweet.
    }
}
```

```
$ cli.cmd passer "This is a message."
$ This is a message.
```

More practical example? Makefile kind of thing?

```
cmd build cmds {
   cmd server sh {
       echo "building server.."
   }

   cmd client sh {
       echo "building client.."
   }

   cmd all cmd {
       server
       client
   }
}
```
