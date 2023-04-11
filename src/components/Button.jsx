import React, { Component } from 'react'
import './Button.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

export default class Button extends Component {
    
    render() {

        const click = this.props.onClick
        const icon = <FontAwesomeIcon icon={this.props.icon} />
        const label = <span>{this.props.label}</span>

        return (
            <button className={`btn ${this.props.extraClasses}`}
                onClick={e => typeof click === 'function' && click(e)}>
                {this.props.label ? label : ''}
                {this.props.icon ? icon : ''}
            </button>
        )
    }
}