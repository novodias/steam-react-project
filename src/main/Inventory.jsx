import React from 'react'
import { Component } from 'react';
import Container from '../components/Container'
import Button from '../components/Button'
import './Inventory.css'
import { faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons';

export default class Inventory extends Component {

    constructor(props) {
        super(props)

        this.updateGrid = this.updateGrid.bind(this)
        this.onClickLeft = this.onClickLeft.bind(this)
        this.onClickRight = this.onClickRight.bind(this)

        const counts = {}
        const user = this.props.inventory
        const assets = user.inventory.assets
        assets.forEach(asset => {
            counts[asset.classid] = (counts[asset.classid] || 0) + 1
        })

        const items = this.updateGrid(0, 30, counts)

        this.state = {
            counts,
            index: 0,
            amountPerPage: 30,
            items
        }
    }

    handleItemInformation(information) {
        this.props.onCellClick && this.props.onCellClick(information)
    }
    
    createCell(item, i, counts) {
        const imgSrc = `https://community.akamai.steamstatic.com/economy/image/${item.icon_url}`
        const alt = item.market_name
        const clickEvent = () => {
            // React doesn't allow to send a object as a parameter
            // Stringify the object and parse it
            // console.log(item)
            const json = JSON.stringify(item)
            this.handleItemInformation(json)
        }

        const amount = (counts || this.state.counts)[item.classid]
        const amountComponent = amount > 1 ?
            <span>{amount}x</span> : null
        
        // let amountComponent = null
        // if (amount > 1) {
        //     amountComponent = <span>{amount}x</span>
        // }

        return (
            <div key={i} className='item-cell' item-classid={item.classid} onClick={clickEvent}>
                <img src={imgSrc} alt={alt} />
                {amountComponent}
            </div>
        )
    }
    
    updateGrid(index = null, amountPerPage = null, counts = null) {
        const user = this.props.inventory
        const inventory = user.inventory
        const components = []

        let i, aPG
        if (!this.state) {
            i = index
            aPG = amountPerPage
        } else {
            i = this.state.index
            aPG = this.state.amountPerPage
        }

        const length = inventory.descriptions.length
        const start = i * aPG
        let end = (i + 1) * aPG
        end = end > length ? length : end
        
        let current_index_items = 0
        for (let current_index_inv = start; current_index_inv < end;
            current_index_inv++) {

            const item = inventory.descriptions[current_index_inv];
            components.push(this.createCell(item, current_index_items, counts))
            current_index_items++
        }

        // await this.setState({items: components})
        return components
    }

    onClickRight() {
        const user = this.props.inventory
        const amount = (this.state.index + 1) * this.state.amountPerPage
        if (amount >= user.inventory.descriptions.length) return

        let index = this.state.index
        this.setState({index: ++index}, () => this.setState({ items: this.updateGrid() }))
    }

    onClickLeft() {
        if (this.state.index === 0) return

        let index = this.state.index
        this.setState({index: --index}, () => this.setState({ items: this.updateGrid() }))
    }

    getPages() {
        const user = this.props.inventory
        return Math.ceil(user.inventory.descriptions.length / this.state.amountPerPage)
    }

    render() {
        const user = this.props.inventory

        if (user && user.public) {
            return (
                <Container>
                    <h1 className='inventory-title'>CSGO Inventory - {user.inventory.total_inventory_count}/5000</h1>
                    <div className='buttons-inventory-wrapper'>
                        <Button icon={faArrowLeft} extraClasses='btn-inv'
                            onClick={this.onClickLeft} />
                        <span>
                            {this.state.index + 1}/{this.getPages()}
                        </span>
                        <Button icon={faArrowRight} extraClasses='btn-inv'
                            onClick={this.onClickRight} />
                    </div>
                    <hr />
                    <div className='items-grid-wrapper'>
                        {this.state.items}
                    </div>
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