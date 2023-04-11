import React, { Component } from 'react'
import Container from '../components/Container'
// import Button from '../components/Button'
// import Attributes from '../components/Attributes';
import './PlayerProfile.css'

class Player {
    constructor(data) {
        this.steamid = data.steamid;
        this.communityvisibilitystate = data.communityvisibilitystate;
        this.profilestate = data.profilestate;
        this.personaname = data.personaname;
        this.commentpermission = data.commentpermission;
        this.profileurl = data.profileurl;
        this.avatar = data.avatar;
        this.avatarmedium = data.avatarmedium;
        this.avatarfull = data.avatarfull;
        this.avatarhash = data.avatarhash;
        this.lastlogoff = data.lastlogoff;
        this.personastate = data.personastate;
        this.primaryclanid = data.primaryclanid;
        this.timecreated = data.timecreated;
        this.personastateflags = data.personastateflags;
    }
}

export default class PlayerProfile extends Component {

    getStatus(state) {
        // eslint-disable-next-line default-case
        switch (state) {
            case 0:
                return 'Offline'

            case 1:
                return 'Online'

            case 2:
                return 'Busy'

            case 3:
                return 'Away'

            case 4:
                return 'Snooze'

            case 5:
                return 'Looking to trade'

            case 6:
                return 'Looking to play'
        }
    }

    getDate(unixTimestamp) {
        const date = new Date(unixTimestamp * 1000)

        const iso = date.toISOString().split('T')

        const formattedDate = iso[0].toString().replaceAll('-', '/')
        const formattedTime = iso[1].split('.')[0]

        const formatted = formattedDate + ' at ' + formattedTime

        return formatted
    }

    createInfoBlock(name, value) {
        return (
            <div>
                <span>{name}</span><strong>{value}</strong>
            </div>
        )
    }

    render() {
        const player = new Player(this.props.player)

        return (
            <Container>
                <div className='f-row'>
                    <img src={player.avatarfull} alt={player.personaname + ' avatar'} />
                    <div>
                        <h1 className='ml-12'>{player.personaname}</h1>
                        <h2 className='ml-12'>{this.getStatus(player.personastate)}</h2>
                    </div>
                    <a label='Open Profile'
                        className='btn btn-right clean-a'
                        href={`https://steamcommunity.com/profiles/${player.steamid}`}
                        target="_blank" rel="noopener noreferrer">
                        Open Profile
                    </a>
                </div>
                <hr />
                {/* <div className='information-container'> */}
                <div className='information-container attribute-wrapper'>
                    { this.createInfoBlock('SteamID', player.steamid) }
                    { this.createInfoBlock('Last time online', this.getDate(player.lastlogoff)) }
                    { this.createInfoBlock('Account creation date', this.getDate(player.timecreated)) }
                </div>
            </Container>
        )
    }
}