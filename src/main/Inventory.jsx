import React from 'react'
import { Component } from 'react';
import Container from '../components/Container'
import './Inventory.css'

export default class Inventory extends Component {

    handleItemInformation(information) {
        this.props.onCellClick && this.props.onCellClick(information)
    }
    
    createCell(item) {
        const imgSrc = `https://community.akamai.steamstatic.com/economy/image/${item.icon_url}`
        const alt = item.market_name
        const clickEvent = () => {
            // console.log(item)
            // React doesn't allow to send a object as a parameter
            // Stringify the object and parse it
            console.log(item)
            const json = JSON.stringify(item)
            this.handleItemInformation(json)
        }

        return (
            <div className='item-cell' onClick={clickEvent}>
                <img src={imgSrc} alt={alt} />
                {/* <span></span> */}
            </div>
        )
    }
    
    createGrid(inventory) {
        const components = []

        for (const item of inventory.descriptions) {
            components.push(this.createCell(item))
        }

        const grid = (
            <div className='items-grid-wrapper'>
                {components}
            </div>
        )

        return grid
    }

    render() {
        const inventory = this.props.inventory
        
        if (inventory.success === 1) {
            return (
                <Container>
                    <h1>CSGO Inventory</h1>
                    <hr />
                    {this.createGrid(inventory)}
                </Container>
            )
        } else {
            return (
                <Container>
                    <h1>Inventory is not public</h1>
                </Container>
            )
        }
    }
}