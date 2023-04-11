import React, { Component } from 'react'
import './Input.css'

export default class Input extends Component {
    
    constructor(props) {
        super(props)

        this.setValue = this.setValue.bind(this)
        this.handleEnter = this.handleEnter.bind(this)
    }
    
    state = {
        value: this.props.value
    }

    setValue(e) {
        this.setState({ value: e.target.value })
        this.props.onChange && this.props.onChange(e.target.value)
    }

    handleEnter(e) {
        if (e.key !== 'Enter') {
            return
        }

        this.props.onEnter(e)
    }
    
    render() {

        return (
            <input type='text' placeholder={this.props.placeholder}
                className={`input-steam-id ${this.props.extraClasses}`}
                value={this.state.value} onChange={this.setValue} onKeyUp={this.handleEnter} />
        )
    }
}