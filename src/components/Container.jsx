import React from "react";
import Loading from "./Loading";
import './Container.css'

const Container = (props) => 
    <div className={`container
    ${props.extraClasses ? props.extraClasses : ""}`}>
        {props.loading ? (<Loading/>) : "" }
        {props.children}
    </div>

export default Container