import React, { Component } from "react";
import SteamFinder from "./SteamFinder";
import PlayerProfile from "./PlayerProfile";
import PlayerBans from "./PlayerBans";
import GamesList from "./GamesList";
import Inventory from "./Inventory";
import Container from "../components/Container";
import Button from "../components/Button";
import ItemPopup from "./ItemPopup";


export default class Main extends Component {
    
    constructor(props) {
        super(props)

        this.state = {
            steamUser: null,
            address: 'http://25.4.109.76:3001/',
            playerProfile: null,
            playerBans: null,
            gamesList: null,
            playerInventory: null,
            itemInformation: null,
            errorComponent: null,
        }

        this.getCSGOInventory = this.getCSGOInventory.bind(this)
        this.showItemInformation = this.showItemInformation.bind(this)
        this.handleOnEnterPressed = this.handleOnEnterPressed.bind(this)
        this.closeItemInformation = this.closeItemInformation.bind(this)

        fetch(this.state.address)
            .catch(() => this.setState({ address: 'http://192.168.0.9:3001/' }))
            .finally(() => {
                fetch(`${this.state.address}user/0/csgo-inventory`)
                    .then(data => {
                        return data.json()
                    })
                    .then(inventory => {
                        this.setState({playerInventory: (<Inventory inventory={inventory} onCellClick={this.showItemInformation} />)})
                    })
                    .catch(reason => {
                        console.log(reason)
                    })
        })
        
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

    closeItemInformation() {
        this.setState({itemInformation: null})
    }

    showItemInformation(data) {
        const item = JSON.parse(data)

        this.setState({itemInformation: <ItemPopup item={item}
            onClick={() => this.closeItemInformation} />})
    }

    getCSGOInventory() {
        fetch(`${this.state.address}user/${this.state.steamUser.steamid}/csgo-inventory`)
            .then(data => {
                return data.json()
            })
            .then(inventory => {
                console.log(inventory)
                this.setState({
                    playerInventory: (<Inventory inventory={inventory}
                        onCellClick={this.showItemInformation} />)
                })
            })
            .catch((reason) => {
                console.log(reason)
            })
    }

    createButtonToGetCSGOInventory() {
        return (
            <Container>
                <Button label={`Get ${this.state.steamUser.personaname} CSGO inventory`}
                    onClick={this.getCSGOInventory}
                    extraClasses='btn-csgo'/>
            </Container>
        )
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
            playerInventory: null
        })

        fetch(`${this.state.address}user/${text}`)
            .then(res => res.json())
            .then(async (data) => {
                if (data.error) {
                    throw new Error(data.error)
                }

                await this.setState({
                    steamUser: data,
                    playerProfile: this.createPlayerProfile(data)
                })

                return data.steamid
            })
            .then(async (steamid) => {
                const response = await fetch(`${this.state.address}user/${steamid}/bans`)
                const data = await response.json()

                this.setState({
                    playerBans: this.createPlayerBans(data)
                })

                return steamid
            })
            .then(async (steamid) => {
                const response = await fetch(`${this.state.address}user/${steamid}/recent`)
                const data = await response.json()

                // console.log(data)
                if (data.games !== undefined) {
                    this.setState({
                        gamesList: this.createGamesList(data.games),
                        playerInventory: this.createButtonToGetCSGOInventory()
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
                {this.state.playerProfile}
                {this.state.playerBans}
                {this.state.gamesList}
                {this.state.playerInventory}
                {this.state.itemInformation}
            </div>
        )
    }
}