﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayListNew.DB
{
    class queries
    {
        //******************* create playlist section*******************************************************************************

        /* 
        * query: selectSongs
        * select songs to the playlist by the user Preferences is generate dynamiclly in the file: createPlaylist.xaml.cs
        * in function 'createQuery'
        */


        // insert playlist name from user to the playlists table
        public static string insertNewPlaylist = @"INSERT INTO playlistgame.playlists(playlists.playlist_name) VALUES ('{0}')";

        // get playlist id by playlist name

        public static string getPlaylistIdByPlaylistName = @"SELECT playlists.playlist_id 
                                                        FROM playlistgame.playlists
                                                        WHERE playlists.playlist_name = '{0}'";


        /*SELECT playlists.playlist_id 
                                                                    FROM playlistgame.playlists
                                                                    inner join user_to_playlists
                                                                    ON playlists.playlist_id = user_to_playlists.playlist_id
                                                                    WHERE user_id = {0} AND playlist_name = '{1}'";
                */
                
        public static string getPlaylistNameAndUserNByPlId = @"SELECT playlists.playlist_name, full_name
                                                    FROM playlistgame.playlists
                                                    inner join user_to_playlists 
                                                    ON playlists.playlist_id = user_to_playlists.playlist_id
                                                    inner join users
                                                    ON user_to_playlists.user_id = users.user_id
                                                    where playlists.playlist_id={0}";

        // create playlist and insert all songs choosed
        public static string creartPlaylist = @"INSERT INTO playlistgame.songs_to_playlist(songs_to_playlist.playlist_id, songs_to_playlist.song_id)
                                               VALUES ('{0}', '{1}')";

        // create row for user id and playlist id and insert to user_tp_playlists table
        public static string crearteUserAndPlaylist = @"INSERT INTO playlistgame.user_to_playlists(user_to_playlists.playlist_id, user_to_playlists.user_id)
                                                      VALUES ('{0}', '{1}')";


        // use to queryTocheckIfPlNameExsistToUser and to get name by user id and playlist name
        public static string queryTocheckIfPlNameExsistToUser = @"SELECT playlists.playlist_name
                                                                FROM playlistgame.playlists
                                                                inner join user_to_playlists 
                                                                ON playlists.playlist_id = user_to_playlists.playlist_id
                                                                where user_id ={0} and playlist_name='{1}'";


        public static string getSongsByPlaylistId = @"SELECT songs.id 
                                                    FROM playlistgame.songs
                                                    inner join songs_to_playlist
                                                    ON songs.id = songs_to_playlist.song_id
                                                    WHERE playlist_id = {0};";
        //******************* END of create playlist section*******************************************************************************



        //******************* Users section**************************************************************************************

        // get user id when user logged in and to check if email exsits
        public static string getUserId = @"SELECT users.user_id
                                            FROM playlistgame.users
                                            WHERE email = '{0}';";

        // insert new user
        public static string insertNewUser = @"INSERT INTO playlistGame.users (Email, Password, Full_name)
                                              Values('{0}','{1}', '{2}');";

      
        // to check if password entered is correct
        public static string toCheckPassword = "SELECT users.password from playlistGame.users WHERE email='{0}';";

        //******************* END of Users section**************************************************************************************





        //******************* show song section**************************************************************************************

        // show playlist songs
        public static string getAllplaylistSongs = @"SELECT songs.name, artists.name, albums.name, songs.id, songs.duration
                                                    FROM playlistgame.songs
                                                    INNER JOIN playlistgame.albums ON albums.id = songs.albumId
                                                    INNER JOIN playlistgame.artists ON artists.id = songs.artistId
                                                    Where songs.id IN (SELECT songs_to_playlist.song_id
                                                                     From playlistgame.songs_to_playlist
                                                                     WHERE songs_to_playlist.playlist_id= '{0}');";

        //******************* show song section**************************************************************************************




        //******************* show playlist (friends and my) section**************************************************************************************

        public static string getAllUserPlaylists = @"SELECT playlists.playlist_name, playlists.playlist_id, COUNT(*) as Num
                                                    FROM playlistgame.playlists
                                                    INNER JOIN playlistgame.user_to_playlists ON playlists.playlist_id = user_to_playlists.playlist_id
                                                    INNER JOIN playlistgame.songs_to_playlist ON playlists.playlist_id = songs_to_playlist.playlist_id
                                                    WHERE user_id='{0}'
                                                    group by playlist_id
				                                    ORDER BY creation_date DESC;";



        // 
        public static string getAllFriendsPlaylists = @"SELECT p.playlist_name, p.playlist_id, u.full_name, COUNT(*) as Num 
											FROM playlists p
                                            JOIN songs_to_playlist sp 
                                            ON p.playlist_id = sp.playlist_id
                                            JOIN songs s
                                            ON sp.song_id = s.id
                                            JOIN user_to_playlists up 
                                            ON p.playlist_id = up.playlist_id
                                            JOIN users u
                                            ON up.user_id = u.user_id
                                            WHERE u.email IN ({0})
											group by p.playlist_name, p.playlist_id, u.full_name
											ORDER BY creation_date DESC;";

        // 
        public static string countFriendPlaylistNum = @"select count(fpl.playlist_id) as pl_counter
                                            from (
                                            SELECT p.playlist_name, p.playlist_id, u.full_name, COUNT(*) as Num 
											FROM playlists p
                                            JOIN songs_to_playlist sp 
                                            ON p.playlist_id = sp.playlist_id
                                            JOIN songs s
                                            ON sp.song_id = s.id
                                            JOIN user_to_playlists up 
                                            ON p.playlist_id = up.playlist_id
                                            JOIN users u
                                            ON up.user_id = u.user_id
                                            WHERE u.email IN ({0})
											group by p.playlist_name, p.playlist_id, u.full_name
											ORDER BY creation_date DESC
                                            ) as fpl";

       


        //******************* show playlist(friends and my) section**************************************************************************************




        //******************* delete section**************************************************************************************

        // when delete playlist - delete the playlist from tables: playlists, songs_to_playlist, user_to_playlists
        public static string deletePlaylist = @"DELETE FROM playlists WHERE playlist_id='{0}';";

        public static string deleteSongsByPlaylist = @"DELETE FROM songs_to_playlist WHERE playlist_id='{0}';";


        public static string deletePlaylistToUser = @"DELETE FROM user_to_playlists WHERE playlist_id='{0}';";


        public static string deleteSongFromPlaylist = @"DELETE FROM songs_to_playlist WHERE song_id='{0}' AND playlist_id='{1}';";


        public static string afterDeletingSongCheckPlaylistSongs = @"DELETE FROM playlistgame.playlists
                                                                WHERE (SELECT COUNT(*) as Num
		                                                                FROM songs_to_playlist
		                                                                WHERE playlist_id = {0}) = 0
                                                                AND playlist_id = {0};";

        public static string afterDeletingSongCheckUTP = @"DELETE FROM playlistgame.user_to_playlists
                                                                WHERE (SELECT COUNT(*) as Num
		                                                                FROM playlists
		                                                                WHERE playlist_id = {0}) = 0
                                                                AND playlist_id = {0};";

        //******************* end of delete section**************************************************************************************
    }

}
