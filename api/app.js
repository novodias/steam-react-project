const express = require('express')
const app = express()
const port = Number.parseInt(process.env.SERVERPORT)
const cors = require('cors')
app.use(cors())

const SteamClient = require('./steam/steamClient')
const key = process.env.STEAMAPIKEY
const client = new SteamClient(key)

if (!port) {
    throw new Error('Port was not defined')
}

app.get('/', (req, res) => {
    res.send('Ok')
})

app.get('/user/:steamid', (req, res, next) => {
    const id = req.params.steamid
    client.getPlayerSummaries(id)
        // .then((response) => {
        //     return response.json()
        // })
        .then((data) => {
            res.send(data)
        })
        .catch((reason) => {
            res.status(400).send({error: 'User not found'})

            // next(reason)
        })
})

app.get('/user/:steamid/recent', (req, res) => {
    const id = req.params.steamid
    const count = req.query.count ? parseInt(req.query.count) : 5
    client.getRecentlyPlayedGames(id, count)
        // .then((response) => {
        //     return response.json()
        // })
        .then((data) => {
            console.log(data)
            res.send(data)
        })
        .catch((reason) => {
            throw reason
        })
})

app.get('/user/:steamid/bans', (req, res) => {
    const id = req.params.steamid
    client.getPlayerBans(id)
        .then((data) => {
            res.send(data)
        })
        .catch((reason) => {
            throw reason
        })
})

app.listen(port, () => {
    console.log('Server listening on port: ' + port)
})