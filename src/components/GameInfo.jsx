import React, { Component } from 'react'
import Container from './Container'
import './GameInfo.css'

export default class GameInfo extends Component {
    
    getImageSource(appId) {
        // Since the API returns a small icon, it's better to get directly from steam
        // https://cdn.akamai.steamstatic.com/steam/apps/930210/header.jpg
        return `https://cdn.akamai.steamstatic.com/steam/apps/${appId}/header.jpg`
    }

    getPlaytime(minutes) {
        const getMinutes = (minutes) => minutes + (minutes > 1 ? ' minutes' : ' minute')
        const getHours = (hours) => hours + (hours > 1 ? ' hours' : ' hour')

        const hours = Math.floor(minutes / 60)
        return minutes > 60 ? getHours(hours) : getMinutes(minutes)
    }

    createInfoBlock(name, value) {
        return (
            <div>
                <span>{name}</span><strong>{value}</strong>
            </div>
        )
    }
    
    render() {
        const game = this.props.game

        return (
            <Container extraClasses='container-game-info'>
                <div>
                    <a className='game-info-title'
                        href={`https://store.steampowered.com/app/${game.appid}`}>
                        {game.name}
                    </a>
                    <div className='game-info-container'>
                        <a className='clean-a'
                            href={`https://store.steampowered.com/app/${game.appid}`}>
                            <img src={this.getImageSource(game.appid)} alt={`${game.name} header`}
                                className='header-img'/>
                        </a>
                        <div className='game-info-details attribute-wrapper'>
                            {this.createInfoBlock('App ID', game.appid)}
                            {this.createInfoBlock('Playtime forever', this.getPlaytime(game.playtime_forever))}
                            {this.createInfoBlock('Playtime in 2 weeks', this.getPlaytime(game.playtime_2weeks))}
                        </div>
                    </div>
                </div>
            </Container>
        )
    }
}