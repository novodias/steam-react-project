import React, { Component } from "react";
import SteamFinder from "./SteamFinder";
import PlayerProfile from "./PlayerProfile";
import PlayerBans from "./PlayerBans";
import GamesList from "./GamesList";

export default class Main extends Component {
    
    constructor(props) {
        super(props)

        this.handleOnEnterPressed = this.handleOnEnterPressed.bind(this)

        fetch(this.state.address)
            .catch(() => this.setState({address: 'http://192.168.0.9:3001/'}))
    }

    state = {
        // steamId: null,
        address: 'http://25.4.109.76:3001/',
        playerProfile: null,
        playerBans: null,
        gamesList: null,
        errorComponent: null,
    }

    showErrorMessage(message) {
        let timer = this.state.timer
        clearTimeout(timer)
        this.setState({timer})

        const component = (
            <div className='steam-finder-error'>
                <span>{message}</span>
            </div>
        )
        
        this.setState({errorComponent: component})
        
        timer = setTimeout(() => {
            this.setState({errorComponent: null, timer: null})
        }, 1000 * 10)

        this.setState({timer})
    }

    createPlayerProfile(data) {
        return (
            <PlayerProfile player={data} />
        )
    }

    createPlayerBans(data) {
        return (
            <PlayerBans bans={data} />
        )
    }

    createGamesList(data) {
        return (
            <GamesList games={data} />
        )
    }

    handleOnEnterPressed(text) {        
        if (text.length === 0) {
            this.showErrorMessage('The input is empty')
            return
        }
        
        if (text.includes(' ')) {
            this.showErrorMessage('The input has spaces, please remove it')
            return
        }

        this.setState({
            playerProfile: null,
            playerBans: null,
            gamesList: null,
        })

        fetch(`http://192.168.0.9:3001/user/${text}`)
            .then(res => res.json())
            .then(async (data) => {
                if (data.error) {
                    throw new Error(data.error)
                }

                await this.setState({
                    playerProfile: this.createPlayerProfile(data)
                })

                return data.steamid
            })
            .then(async (steamid) => {
                const response = await fetch(`http://192.168.0.9:3001/user/${steamid}/bans`)
                const data = await response.json()

                this.setState({
                    playerBans: this.createPlayerBans(data)
                })

                return steamid
            })
            .then(async (steamid) => {
                const response = await fetch(`http://192.168.0.9:3001/user/${steamid}/recent`)
                const data = await response.json()

                // console.log(data)
                if (data.games !== undefined) {
                    this.setState({
                        gamesList: this.createGamesList(data.games)
                    })
                    return
                }

            })
            .catch((reason) => {
                // this.showErrorMessage(reason)
                console.log(reason)
                this.showErrorMessage(`${reason}`)
            })
    }
    
    render() {
        return (
            <div>
                <SteamFinder onEnter={this.handleOnEnterPressed}
                    error={this.state.errorComponent} />
                { this.state.playerProfile }
                { this.state.playerBans }
                { this.state.gamesList }
            </div>
        )
    }
}