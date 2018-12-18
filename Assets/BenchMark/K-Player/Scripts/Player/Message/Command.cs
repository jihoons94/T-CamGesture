
public class ContentLoadCommand
{
    public KContent Content { get; set; }
}

public class ContentUnloadCommand { }

public class ContentShowCommand
{
    public bool Show { get; set; }
}

public class PlayerStartCommand { }

public class PlayerPauseCommand { }

public class PlayerStopCommand { }

public class PlayerSpeedCommand {
    public float Speed { get; set; }
}