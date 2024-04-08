example = {
    name = "Example",
    description = "This is an example of a mod",
    author = "Author",
    version = "1.0",
    dependencies = {
        "ExampleDependency"
    }
}
    
    
function example:load()
    print("Example mod loaded")
    Events.Server.WaitingForPlayers.add(self.onWaitingForPlayers)
end

function example:unload()
    print("Example mod unloaded")
    Events.Server.WaitingForPlayers.remove(self.onWaitingForPlayers)
end

function example:onWaitingForPlayers()
    print("Example mod waiting for players")
end

example:load()