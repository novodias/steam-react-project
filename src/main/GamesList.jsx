import React, { Component } from "react";
import GameInfo from "../components/GameInfo";

export default class GamesList extends Component {
    createComponents() {
        const components = []
        const games = this.props.games
        console.log(games)

        for (let i = 0; i < games.length; i++) {
            components.push(
                <GameInfo game={games[i]} />
            )
        }

        return components
    }
    
    render() {
        return (
            <div>
                {this.createComponents()}
            </div>
        )
    }
}