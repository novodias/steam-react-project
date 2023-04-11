import React from "react";
import './Container.css'

const Container = (props) => 
    <div className={`container ${props.extraClasses ? props.extraClasses : ''}`}>
        {props.children}
    </div>

export default Container