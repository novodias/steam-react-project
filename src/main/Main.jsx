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
            address: 'http://25.4.109.76:3001',
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
            .catch(() => this.setState({ address: 'http://192.168.0.9:3001' }))
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
        fetch(`${this.state.address}/user/${this.state.steamUser.steamid}/csgo-inventory`)
            .then(data => {
                return data.json()
            })
            .then(user => {
                this.setState({
                    playerInventory: (<Inventory inventory={user}
                        onCellClick={this.showItemInformation} />)
                })
            })
            .catch((reason) => {
                console.log(reason)
            })
    }

    createButtonToGetCSGOInventory(data) {
        return (
            <Container>
                <Button label={`Get ${data.personaname} CSGO inventory`}
                    onClick={this.getCSGOInventory}
                    extraClasses='btn-csgo'/>
            </Container>
        )
    }

    async fetchJsonAsync(url, init = undefined)
    {
        try {
            const response = await fetch(url, init)
            const data = response.json();

            if (data.error)
            {
                throw data.error    
            }

            return data;
        } catch (error) {
            throw error
        }
    }

    async handleOnEnterPressed(text) {        
        if (text.length === 0) {
            this.showErrorMessage('The input is empty')
            return
        }
        
        if (text.includes(' ')) {
            this.showErrorMessage('The input has spaces, please remove it')
            return
        }

        const container = <Container loading />

        try {
            await this.setState({
                playerProfile: container,
                playerBans: container,
                gamesList: container,
                playerInventory: container
            })

            const data = await this.fetchJsonAsync(`${this.state.address}/user/${text}`)
            await this.setState({
                steamUser: data,
                playerProfile: <PlayerProfile player={data} />,
                playerInventory: this.createButtonToGetCSGOInventory(data)
            })

            const bans = await this.fetchJsonAsync(`${this.state.address}/user/${data.steamid}/bans`)
            await this.setState({
                playerBans: <PlayerBans bans={bans}/>
            })

            const gamesList = await this.fetchJsonAsync(`${this.state.address}/user/${data.steamid}/recent`)
            if (gamesList.games !== undefined) {
                this.setState({
                    gamesList: <GamesList games={gamesList} />
                })
                return
            }
        } catch (error) {
            console.log(error)
            this.showErrorMessage(`${error}`)
            this.setState({
                playerProfile: null,
                playerBans: null,
                gamesList: null,
                playerInventory: null
            })
        }

        // fetch(`${this.state.address}user/${text}`)
        //     .then(res => res.json())
        //     .then(async (data) => {
        //         if (data.error) {
        //             throw new Error(data.error)
        //         }

        //         await this.setState({
        //             steamUser: data,
        //             playerProfile: <PlayerProfile player={data} />,
        //             playerInventory: this.createButtonToGetCSGOInventory(data)
        //         })

        //         return data.steamid
        //     })
        //     .then(async (steamid) => {
        //         const response = await fetch(`${this.state.address}user/${steamid}/bans`)
        //         const data = await response.json()

        //         this.setState({
        //             playerBans: <PlayerBans bans={data} />
        //         })

        //         return steamid
        //     })
        //     .then(async (steamid) => {
        //         const response = await fetch(`${this.state.address}user/${steamid}/recent`)
        //         const data = await response.json()

        //         // console.log(data)
        //         if (data.games !== undefined) {
        //             this.setState({
        //                 gamesList: <GamesList games={data} />
        //             })
        //             return
        //         }

        //     })
        //     .catch((reason) => {
        //         console.log(reason)
        //         this.showErrorMessage(`${reason}`)
        //         this.setState({
        //             playerProfile: null,
        //             playerBans: null,
        //             gamesList: null,
        //             playerInventory: null
        //         })
        //     })
    }
    
    render() {
        return (
            <div>
                <SteamFinder onEnter={async (e) => this.handleOnEnterPressed(e)}
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
