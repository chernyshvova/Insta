using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace instarm
{
    class CommandLine
    {
        const string PostCmd = "-post";
        const string LikeCmd = "-like";

        const string ByTag = "--tag";
        const string Single = "--single";

        const string CommentCmd = "-comment";
        const string AvatarCmd = "-avatar";
        const string HelpCmd = "-help";
        Commands cmd = new Commands();
        MassAccountManager manager = new MassAccountManager();
        List<string> input = new List<string>();
        enum Keys
        {
            SetPostByTag,
            SetPostSingle,
            LikeMediaByTag,
            LikeMediaSingle,
            CommentMedia,
            ChangeAvatar,
            Help,
            Error
        }

        public async Task Execute(string []args)
        {
            foreach (var item in args)
            {
                input.Add(item);
            }

            Keys command = AnalyseCmd();

            if (command!=Keys.Error)
            {
                if (command == Keys.Help)
                {
                    WriteHelp();

                }
                else
                {
                    await RunCommand(command);
                }
            }
            else
            {
                Console.WriteLine("Can't recognize commannd.Please type --help to recive available commands");
            }

        }

        async Task RunCommand(Keys command)
        {
            string message = "";
            switch (command)
            {
                case Keys.SetPostByTag:
                    for (int i = 4; i != input.Count; i++)
                    {
                        message += input[i] + " ";
                    }
                    await manager.SetPostByTag(input[2], input[3], message);
                    break;
                case Keys.SetPostSingle:
                    for (int i = 4; i != input.Count; i++)
                    {
                        message += input[i] + " ";
                    }
                    await manager.SetPostSingle(input[2], input[3], message);
                    break;
                case Keys.LikeMediaByTag:
                    await manager.LikeMediaByTag(input[2], input[3]);
                    break;
                case Keys.LikeMediaSingle:
                    await manager.LikeMediaSingle(input[2], input[3]);
                    break;
                case Keys.CommentMedia:
                    for (int i = 3; i != input.Count; i++)
                    {
                        message += input[i] + " ";
                    }
                    await manager.CommentMedia(input[1], input[2], message);
                    break;
                case Keys.ChangeAvatar:
                    await manager.ChangeAvatar(input[1], input[2]);
                    break;
                default:
                    cmd.Error();
                    break;

            }
        }


        private Keys AnalyseCmd()
        {
            if (input[0].Equals(PostCmd))
            {
                if (input[1].Equals(ByTag))
                {
                    if (input.Count >= 5)
                    {
                        return Keys.SetPostByTag;
                    }
                    else
                    {
                        return Keys.Error;
                    }
                }
                if (input[1].Equals(Single))
                {
                    if (input.Count >= 5)
                    {
                        return Keys.SetPostSingle;
                    }
                    else
                    {
                        return Keys.Error;
                    }
                }
                else
                {
                    return Keys.Error;
                }
                                            
            }

            if (input[0].Equals(LikeCmd))
            {
                if (input[1].Equals(ByTag))
                {
                    if (input.Count == 4)
                    {
                        return Keys.LikeMediaByTag;
                    }
                    else
                    {
                        return Keys.Error;
                    }
                }
                if (input[1].Equals(Single))
                {
                    if (input.Count == 4)
                    {
                        return Keys.LikeMediaSingle;
                    }
                    else
                    {
                        return Keys.Error;
                    }
                }
                else
                {
                    return Keys.Error;
                }

            }

            if (input[0].Equals(CommentCmd))
            {
                if (input.Count >= 4)
                {
                    return Keys.CommentMedia;
                }
                else
                {
                    return Keys.Error;
                }
            }
            if (input[0].Equals(AvatarCmd))
            {
                if (input.Count == 3)
                {
                    return Keys.ChangeAvatar;
                }
                else
                {
                    return Keys.Error;
                }
            }
            if (input[0].Equals(HelpCmd))
            {
                return Keys.Help;
            }

            else
            {
                return Keys.Error;
            }

        }

        private class Commands
        {   // Dummy class, in future delete it
            public void Error()
            {
                Console.WriteLine("ERROR!!!");
            }
        }
        public void WriteHelp()
        {
            Console.WriteLine("--=COMMANDS==--");
            Console.WriteLine("-post --tag dbtag imagename.jpg message \t \t to post image by tag");
            Console.WriteLine("-post --single username imagename.jpg message \t \t to post image by single user");
            Console.WriteLine("-like --tag dbtag mediaUrl \t \t \t \t to like image by tag");
            Console.WriteLine("-like --single username mediaUrl \t \t \t to like image by single user");
            Console.WriteLine("-comment username mediaUrl message \t \t \t to comment image");
            Console.WriteLine("-avatar username image.jpg \t \t \t \t to change profile avatar");
            Console.WriteLine("--=COMMANDS==--");
        }

    }
}
