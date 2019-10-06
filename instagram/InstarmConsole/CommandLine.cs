using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstarmCore;
namespace InstarmConsole
{
    class CommandLine
    {
        const string PostCmd = "-post";
        const string LikeCmd = "-like";
        const string DirectCmd = "-direct";
        const string Path = "--path";

        const string Send = "--send";
        const string Answer= "--answer";
        const string Check = "--check";


        const string FollowCmd = "-follow";
        const string UnfollowCmd = "-unfollow";

        const string LikeExploreCmd = "--mass";
        const string ByTag = "--tag";
        const string Single = "--single";

        const string CommentCmd = "-comment";
        const string AvatarCmd = "-avatar";
        const string HelpCmd = "-help";
        Commands cmd = new Commands();
        AccountManager manager = new AccountManager();
        List<string> input = new List<string>();
        enum Keys
        {
            SetPostByTag,
            SetPostSingle,
            SetPostPath,
            LikeMediaByTag,
            LikeMediaSingle,
            LikeExplore,
            Follow,
            Unfollow,
            CommentMedia,
            ChangeAvatar,
            ChangeAvatarPath,
            DirectAnswer,
            DirectSend,
            DirectCheck,
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
                case Keys.SetPostPath:
                    for (int i = 4; i != input.Count; i++)
                    {
                        message += input[i] + " ";
                    }
                    await manager.SetPostSinglePath(input[2], input[3], message);
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
                case Keys.ChangeAvatarPath:
                    await manager.ChangeAvatarPath(input[2], input[3]);
                    break;
                case Keys.LikeExplore:
                    await manager.ExploreLikeHashtag(input[2], input[3], input[4]);
                    break;
                case Keys.Follow:
                    await manager.FollowUser(input[1], input[2]);
                    break;
                case Keys.Unfollow:
                    await manager.UnFollowUser(input[1], input[2]);
                    break;
                case Keys.DirectSend:
                    for (int i = 4; i != input.Count; i++)
                    {
                        message += input[i] + " ";
                    }
                    await manager.DirectSendMessage(input[2], input[3], message);
                    break;
                case Keys.DirectAnswer:
                    for (int i = 4; i != input.Count; i++)
                    {
                        message += input[i] + " ";
                    }
                    await manager.DirectAnswerMessage(input[2], input[3], message);
                    break;
                case Keys.DirectCheck:
                    await manager.DirectCheckMessages(input[2]);
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
                if (input[1].Equals(Path))
                {
                    if (input.Count >= 5)
                    {
                        return Keys.SetPostPath;
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
                if (input[1].Equals(LikeExploreCmd))
                {
                    if (input.Count == 5)
                    {
                        return Keys.LikeExplore;
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
                if (input[1].Equals(Path))
                {
                    if (input.Count == 4)
                    {
                        return Keys.ChangeAvatarPath;
                    }
                    else
                    {
                        return Keys.Error;
                    }
                }
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
            if (input[0].Equals(FollowCmd))
            {
                if (input.Count == 3)
                {
                    return Keys.Follow;
                }
                else
                {
                    return Keys.Error;
                }
            }
            if (input[0].Equals(UnfollowCmd))
            {
                if (input.Count == 3)
                {
                    return Keys.Unfollow;
                }
                else
                {
                    return Keys.Error;
                }
            }
            if (input[0].Equals(DirectCmd))
            {
                if (input[1].Equals(Send))
                {
                    if (input.Count >= 5)
                    {
                        return Keys.DirectSend;
                    }
                    else
                    {
                        return Keys.Error;
                    }
                }
                if (input[1].Equals(Answer))
                {
                    if (input.Count >= 5)
                    {
                        return Keys.DirectAnswer;
                    }
                    else
                    {
                        return Keys.Error;
                    }
                }
                if (input[1].Equals(Check))
                {
                    if (input.Count == 3)
                    {
                        return Keys.DirectCheck;
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
            Console.WriteLine("-like --mass username hashtag pages(int) \t \t to like world images by hashtag");
            Console.WriteLine("-comment username mediaUrl message \t \t \t to comment image");
            Console.WriteLine("-avatar username image.jpg \t \t \t \t to change profile avatar");
            Console.WriteLine("-follow username targetUsername \t \t \t to follow target by user");
            Console.WriteLine("-unfollow username targetUsername \t \t \t to unfollow target by user");

            Console.WriteLine("-direct --send username targetUsername message \t \t to send message to target from user");
            Console.WriteLine("-direct --answer username targetUsername message \t to send answer message to target from user (don't use if thread NOT exist)");
            Console.WriteLine("-direct --check username \t \t \t \t to check new incoming messages");
            Console.WriteLine("--=COMMANDS==--");
        }

    }
}
