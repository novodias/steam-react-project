import React, { Component } from 'react'
import Container from '../components/Container'
import './PlayerBans.css'

export default class PlayerBans extends Component {

    createBanStatus(name, value) {
        return (
            <div className={value ? 'bg-red' : 'bg-green'}>
                <span>{name}</span><strong>{value ? 'Banned' : 'Not banned'}</strong>
            </div>
        )
    }

    createInfoBlock(name, value) {
        return (
            <div>
                <span>{name}</span><strong>{value}</strong>
            </div>
        )
    }

    capitalizeFirstLetter(string) {
        return string.charAt(0).toUpperCase() + string.slice(1)
    }
    
    render() {
        const status = this.props.bans

        return (
            <Container>
                <h1>Bans status</h1>
                <div className={`player-bans-wrapper attribute-wrapper ${this.props.extraClasses}`}>
                    {this.createBanStatus('VAC banned', status.VACBanned)}
                    {this.createBanStatus('Community banned', status.CommunityBanned)}
                    {this.createInfoBlock('Number of VAC bans', status.NumberOfVACBans)}
                    {this.createInfoBlock('Days since last ban', status.DaysSinceLastBan)}
                    {this.createInfoBlock('Days since last game ban', status.NumberOfGameBans)}
                    {this.createInfoBlock('Economy ban', this.capitalizeFirstLetter(status.EconomyBan))}
                </div>
            </Container>
        )
    }
}
