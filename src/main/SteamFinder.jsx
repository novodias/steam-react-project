import React, { Component } from 'react'
import './SteamFinder.css'
import Input from '../components/Input'
import Button from '../components/Button'
import Container from '../components/Container'
import { faSearch } from '@fortawesome/free-solid-svg-icons'

export default class SteamFinder extends Component {

    constructor(props) {
        super(props)

        this.handleInputOnChange = this.handleInputOnChange.bind(this)
        this.handleOnEnter = this.handleOnEnter.bind(this)
    }
    
    state = {
        inputValue: '',
        // showError: false,
        // errorComponent: null
    }

    handleInputOnChange(value) {
        this.setState({inputValue: value})
    }

    handleOnEnter(e) {
        e.preventDefault()
        // fetch(`http://127.0.0.1:3001/${this.state.inputValue}`)
        //     .then(res => res.json())
        //     .then(json => console.log(json))
        // .catch(() => {this.showErrorMessage('Server API is not online')})

        this.props.onEnter(this.state.inputValue)
    }
    
    render() {
        return (
            <Container>
                <div className='steam-finder-container'>
                    <Input value={this.state.inputValue}
                        onChange={this.handleInputOnChange}
                        onEnter={this.handleOnEnter} extraClasses='steam-finder-input'
                        placeholder='SteamID / ID...'/>
                    <Button
                        extraClasses='steam-finder-btn'
                        icon={faSearch}
                        onClick={this.handleOnEnter} />
                </div>
                { this.props.error }
            </Container>
        )
    }
}