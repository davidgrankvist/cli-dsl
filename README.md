# cli-dsl

A language for creating CLIs.

## About

The idea of CLI DSL is to quickly build project-specific CLIs. The syntax is focused on command structure and the logic is delegated to embedded scripts.

Here's how it works:
- the CLI is defined in a file using a custom syntax
- the `clidsl` interpreter parses the file and executes a given path
- embedded scripts are run as child processes

## Usage

### Quickstart

1. Install `clidsl`
2. Use the `bootstrap` command to generate a wrapper `sh` or `bat` script
```
$ clidsl bootstrap sh
$ ./cli.sh hello
$ hello
```
3. Edit `commands.txt` according to your needs (see the language tour)

#### How does it work?

You can think of `clidsl` as the interpreter and `cli.sh` as your project specific CLI. Running `cli.sh` is the same as

```
$ clidsl commands.txt hello
$ hello
```

### Language tour

Here is a command for printing hello using `sh`.
```
cmd hello sh {
    echo hello
}
```

The command can be executed with `clidsl` like this.
```
$ clidsl commands.txt hello
$ hello
```

Commands can be nested using the `cmds` keyword.
```
cmd build cmds {
    cmd server sh {
        echo Building server..
    }
    cmd client sh {
        echo Building client..
    }
}
```

and a specific path can be executed
```
$ clidsl commands.txt build server
$ Building server..
```

Commands can be combined using the `cmdz` keyword. The combined commands run independently and may use different scripting languages.
```
cmd first sh {
    echo first
}

cmd second bash {
    echo second
}

cmd both cmdz {
    first
    second
}
```

```
$ clidsl commands.txt both
$ first
$ second
```

When commands are nested, the parent command can have its own script by using the `self` keyword.

```
cmd parent cmds {
    cmd self sh {
        echo parent
    }

    cmd child sh {
        echo child
    }
}
```

```
$ clidsl commands.txt parent
$ parent
```

#### Note - unfinished ideas below

Commands can be documented with the `summary` and `arg` keywords.

```
cmd something cmds {
    cmd self sh {
        echo hello
    }

    summary {
        This command does something.
    }

    arg verbose {
        Make the thing verbose.
    }
}
```

When passing in `--help` they are combined to a help text.
```
$ clidsl commands.txt something --help
$ This command does something.
$
$ Parameters:
$ --help Output this text.
$ --verbose Make the thing verbose.
```
