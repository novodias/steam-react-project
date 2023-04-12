import React, { Component } from 'react'
import { faClose } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import './ItemPopup.css'

export default class ItemPopup extends Component {
    createTags(tags) {
        const components = []

        for (const tag of tags) {
            components.push(this.createTag(tag))
        }

        return components
    }
    
    createTag(tag) {
        return (
            <span className='item-tag'>
                {tag.localized_category_name}: {tag.localized_tag_name}
            </span>
        )
    }

    render() {
        const item = this.props.item
        const imgSrc = `https://community.akamai.steamstatic.com/economy/image/${item.icon_url}`
        const click = this.props.onClick

        let button = ''
        if (item.actions) {
            button = (
                <a href={item.actions[0].link}
                    className='clean-a btn'>
                    {item.actions[0].name}
                </a>
            )
        }

        return (
            <div className="item-information-container">
                <div className="item-top-wrapper">
                    <span className="item-title-name">{item.name}</span>
                    <span className='item-title-btn'
                        onClick={click && click()}>
                        <FontAwesomeIcon icon={faClose} />
                    </span>
                </div>
                <div className="item-img-wrapper">
                    <img src={imgSrc} alt={item.name} />
                </div>
                <div className="item-details-wrapper">
                    <h2 style={{ color: `#${item.name_color}` }}>
                        {item.market_name}
                    </h2>
                    <div className='item-details-tags'>
                        {this.createTags(item.tags)}
                    </div>
                    {button}
                </div>
            </div>
        )
    }
}