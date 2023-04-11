import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';

import './fonts/Roboto-Regular.ttf'

import Main from './main/Main';
// import SteamFinder from './main/SteamFinder';
// import PlayerProfile from './main/PlayerProfile';
// import PlayerBans from './main/PlayerBans';
// import GamesList from './main/GamesList';


// const json = JSON.parse(`{
//   "response": {
//   "players": [
//     {
//       "steamid": "76561198132808518",
//       "communityvisibilitystate": 3,
//       "profilestate": 1,
//       "personaname": "thekronn0s",
//       "commentpermission": 1,
//       "profileurl": "https://steamcommunity.com/id/thekronn0s/",
//       "avatar": "https://avatars.akamai.steamstatic.com/d8f200737ae678a010ff501eb85a9a288fc24f97.jpg",
//       "avatarmedium": "https://avatars.akamai.steamstatic.com/d8f200737ae678a010ff501eb85a9a288fc24f97_medium.jpg",
//       "avatarfull": "https://avatars.akamai.steamstatic.com/d8f200737ae678a010ff501eb85a9a288fc24f97_full.jpg",
//       "avatarhash": "d8f200737ae678a010ff501eb85a9a288fc24f97",
//       "lastlogoff": 1680437586,
//       "personastate": 3,
//       "primaryclanid": "103582791462516598",
//       "timecreated": 1397253327,
//       "personastateflags": 0
//     }
//   ]
// }
// }`)
// const player = json.response.players[0]
// const recent = JSON.parse(`{"total_count":4,"games":[{"appid":930210,"name":"HuniePop 2: Double Date","playtime_2weeks":628,"playtime_forever":840,"img_icon_url":"47b36d67e5f693477dd3da5636ade76d7ef33d8f","playtime_windows_forever":840,"playtime_mac_forever":0,"playtime_linux_forever":0},{"appid":105600,"name":"Terraria","playtime_2weeks":5,"playtime_forever":607,"img_icon_url":"858961e95fbf869f136e1770d586e0caefd4cfac","playtime_windows_forever":607,"playtime_mac_forever":0,"playtime_linux_forever":0},{"appid":322330,"name":"Don't Starve Together","playtime_2weeks":4,"playtime_forever":195,"img_icon_url":"a80aa6cff8eebc1cbc18c367d9ab063e1553b0ee","playtime_windows_forever":187,"playtime_mac_forever":0,"playtime_linux_forever":0},{"appid":730,"name":"Counter-Strike: Global Offensive","playtime_2weeks":1,"playtime_forever":106421,"img_icon_url":"69f7ebe2735c366c65c0b33dae00e12dc40edbe4","playtime_windows_forever":23704,"playtime_mac_forever":0,"playtime_linux_forever":6}]}`)
// const bans = JSON.parse(`{
//   "players": [
//       {
//           "SteamId": "76561198132808518",
//           "CommunityBanned": false,
//           "VACBanned": false,
//           "NumberOfVACBans": 0,
//           "DaysSinceLastBan": 0,
//           "NumberOfGameBans": 0,
//           "EconomyBan": "none"
//       }
//   ]
// }`).players[0]

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <Main />
    {/* <SteamFinder />
    <PlayerProfile player={player} />
    <PlayerBans bans={bans} />
    <GamesList games={recent.games} /> */}
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
