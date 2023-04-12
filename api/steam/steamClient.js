const ISteamUser = require('./interfaces/ISteamUser')
const IPlayerService = require('./interfaces/IPlayerService')
const Inventory = require('./scraper/Inventory')

class SteamClient {

    steamUserInterface = new ISteamUser()
    steamPlayerInterface = new IPlayerService()

    inventoryScrape = new Inventory()

    constructor(key) {
        this.base = 'api.steampowered.com'
        this.key = key
    }

    static verifyInstance(steamClient) {
        if (!(steamClient instanceof SteamClient)) {
            throw new Error('steamClient is not a instance of SteamClient')
        }

        return steamClient
    }

    getVersionAndMethod (version, method) {
        return { version, method }
    }

    createUrlBuilder(interfaceName, methodName, version, format) {
        const builder = {
            // url: `https://${this.base}/${interfaceName}/${methodName}/${version}/?key=${this.key}`,
            baseUrl: `https://${this.base}`,
            interface: interfaceName,
            method: methodName,
            version: version,
            format: format,
            key: this.key,
            query: [],
            json: {},
            addQuery: function (key, value) {
                this.query.forEach(q => {
                    if (q.contains(key)) {
                        return
                    }
                })

                this.query.push(`&${key}=${value}`)
            },
            setJson: function (key, value) {
                this.json[key] = value
            },
            build: function () {
                let request = this.baseUrl + `/${this.interface}/${this.method}/${this.version}/?key=${this.key}&format=${this.format}`
                
                // adds all queries to url
                request += this.query.reduce((current, next) => current + next, '')

                // verifies if json is not empty
                if (Object.keys(this.json).length !== 0) {
                    const input_json = JSON.stringify(this.json)
                    request += `&input_json=${input_json}`
                }

                return request
            }
        }

        return builder
    }

    async getSteamId(vanityUrl) {
        try {
            const response = await this.steamUserInterface.resolveVanityUrl(this, vanityUrl)
            const data = await response.json()
            
            if (data.response.success !== 1) {
                throw Error('User was not found')
            }

            const obj = {
                steamid: data.response.steamid,
                vanityurl: vanityUrl
            }
    
            return obj
        } catch (error) {
            throw error
        }
    }

    isNumber = (number) => !isNaN(parseInt(number))

    async getPlayerSummaries(steamId) {
        try {
            // Verifies if the steamId is a string or a number
            // let id = Number.parseInt(steamId)
            let id = steamId
            let obj = null
            
            // Not a number, do an API call to get steamId
            // if (isNaN(id)) {
            //     id = await this.getSteamId(steamId)
            // }
            if (!this.isNumber(id)) {

                // Returns an object with steamid and vanityurl
                obj = await this.getSteamId(steamId)
                id = obj.steamid
            }

            const response = await this.steamUserInterface.getPlayerSummaries(this, id)
            const json = await response.json()
            const player = json.response.players[0]

            if (obj !== null) {
                player.vanityurl = obj.vanityurl
            }

            return player
        } catch (error) {
            throw error
        }
    }

    async getRecentlyPlayedGames(steamId, count) {
        try {
            const response = await this.steamPlayerInterface.getRecentlyPlayedGames(this, steamId, count)
            return (await response.json()).response
        } catch (error) {
            throw error
        }
    }

    async getPlayerBans(steamids) {
        try {
            const response = await this.steamUserInterface.getPlayerBans(this, steamids)
            return (await response.json()).players[0]
        } catch (error) {
            throw error
        }
    }

    async getPlayerInventory(steamid, appid, count) {
        const request = this.inventoryScrape.getUrl(
            steamid,
            appid,
            this.inventoryScrape.languages.en,
            count
        )

        const response = await this.getAsync(request)
        return await response.json()
    }

    async getAsync(request) {
        try {
            const response = await fetch(request)
            return response
        } catch (error) {
            throw error
        }
    }
}

module.exports = SteamClient