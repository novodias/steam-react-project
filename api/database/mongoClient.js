const { MongoClient, ServerApiVersion } = require('mongodb')

// const mongoClient = require('mongodb').MongoClient

class SteamMongoDatabase {
    
    // url = 'mongodb://localhost:27017/mydb'
    uri = process.env.MONGODBURI
    
    collections = Object.freeze({
        STEAM_USERS: 'SteamUsers',
        STEAM_USERS_INVENTORY: 'SteamUsersInventory'
    })

    client = new MongoClient(this.uri, {
        serverApi: {
            version: ServerApiVersion.v1,
            strict: true,
            deprecationErrors: true,
        }
    })

    /**
     * How the collection SteamUsers should be;
     * 
     * steamid
     * vanityurl
     * personaname
     * profileurl
     * avatarhash
     * timecreated
     * lastupdate: iso-date
     */

    /**
     * How the collection SteamUsersInventory should be;
     * 
     * steamid
     * inventory: {
     *    assets: [{...}],
     *    descriptions: [{...}],
     *    total_inventory_count
     * }
     * lastupdate: iso-date
     */

    async run() {
        try {
            await this.client.connect()
            await this.client.db("admin").command({ ping: 1 })
            console.log("Successfully connected to database")
        } finally {
            await this.client.close()
        }
    }
    
    async createCollections() {
        
        try {
            await this.client.connect()
            await this.client.db('mydb').createCollection(this.collections.STEAM_USERS)
        } catch (error) {
            if (error.codeName === 'NamespaceExists') {
                console.log("Collection 'SteamUsers' already exists, ignoring create collection...")
            } else {
                throw error
            }
        } finally {
            await this.client.close()
        }

        try
        {
            await this.client.connect()
            await this.client.db('mydb').createCollection(this.collections.STEAM_USERS_INVENTORY)
        } catch (error) {
            if (error.codeName === 'NamespaceExists') {
                console.log("Collection 'SteamUsersInventory' already exists, ignoring create collection...")
            } else {
                throw error
            }
        } finally {
            await this.client.close()
        }


        // this.client.connect(this.url, function (err, db) {
        //     if (err) {
        //         throw err
        //     }

        //     const dbo = db.db('mydb')
        //     dbo.createCollection("SteamUsers", function (err, res) {
        //         if (err) {
        //             if (err.ok === 0) {
        //                 console.log("Collection 'SteamUsers' already exists, ignoring create collection...")
        //                 return
        //             }

        //             throw err
        //         }
    
        //         console.log("Collection 'SteamUsers' created")
        //     })
            
        //     dbo.createCollection("SteamUsersInventory", function (err, res) {
        //         if (err) {
        //             if (err.ok === 0) {
        //                 console.log("Collection 'SteamUsersInventory' already exists, ignoring create collection...")
        //                 return
        //             }
        //             throw err
        //         }
    
        //         console.log("Collection 'SteamUsersInventory' created")
        //     })
    
        //     db.close()
        // })
    }

    // checkCollections(collection) {
    //     if (collection !== this.collections.STEAM_USERS ||
    //         collection !== this.collections.STEAM_USERS_INVENTORY) {
    //         throw new Error("Parameter [collection] doesn't exist in the database")
    //     }
    // }

    _createDate(value) {
        if (!value) return null

        return new Date(value * 1000).toISOString()
    }

    _createUserObject(object) {
        return {
            _id: object.steamid,
            vanityurl: object.vanityurl,
            personaname: object.personaname,
            personastate: object.personastate,
            personastateflags: object.personastateflags,
            profileurl: object.profileurl,
            profilestate: object.profilestate,
            commentpermission: object.commentpermission,
            avatar: object.avatar,
            avatarmedium: object.avatarmedium,
            avatarfull: object.avatarfull,
            avatarhash: object.avatarhash,
            primaryclanid: object.primaryclanid,
            communityvisibilitystate: object.communityvisibilitystate,
            lastlogoff: this._createDate(object.lastlogoff),
            timecreated: this._createDate(object.timecreated),
            lastupdate: new Date().toISOString()
        }
    }

    _createInventoryObject(object, steamid) {
        if (!object) {
            return {
                _id: steamid,
                public: false,
                lastupdate: new Date().toISOString()
            }
        }

        return {
            _id: steamid,
            public: true,
            inventory: {
                assets: object.assets,
                descriptions: object.descriptions,
                total_inventory_count: object.total_inventory_count
            },
            lastupdate: new Date().toISOString()
        }
    }

    async insertUser(object) {
        try {
            const obj = this._createUserObject(object)
            
            await this.client.connect()
            const db = await this.client.db('mydb')
            const col = await db.collection(this.collections.STEAM_USERS)
            const result = await col.insertOne(obj)

            console.log(`[Database][${this.collections.STEAM_USERS}] ${result.insertedId} user inserted`)
        } catch (error) {
            throw error
        } finally {
            await this.client.close()
        }

        // this.client.connect(this.url, (err, db) => {
        //     if (err) {
        //         throw err
        //     }
            
        //     // const obj = {
        //     //     _id: object.steamid,
        //     //     vanityurl: object.vanityurl,
        //     //     personaname: object.personaname,
        //     //     profileurl: object.profileurl,
        //     //     avatarhash: object.avatarhash,
        //     //     timecreated: new Date(object.timecreated * 1000).toISOString(),
        //     //     lastupdate: new Date().toISOString()
        //     // }

        //     const obj = this._createUserObject(object)

        //     const dbo = db.db('mydb')
        //     dbo.collection(this.collections.STEAM_USERS)
        //         .insertOne(obj,(err, res) => {
        //             if (err) {
        //                 throw err
        //             }

        //             console.log(`[Database][${this.collections.STEAM_USERS}] 1 user inserted`)
        //             db.close()
        //     })
        // })
    }

    async insertInventory(object, steamid) {
        const obj = this._createInventoryObject(object, steamid)
        
        try { 
            await this.client.connect()
            const db = await this.client.db('mydb')
            const col = await db.collection(this.collections.STEAM_USERS_INVENTORY)
            const result = await col.insertOne(obj)

            console.log(`[Database][${this.collections.STEAM_USERS_INVENTORY}] ${result.insertedId} user inventory inserted`)
        } catch (error) {
            throw error
        } finally {
            await this.client.close()
        }

        return obj

        // this.client.connect(this.url, (err, db) => {
        //     if (err) {
        //         throw err
        //     }
            
        //     // const obj = {
        //     //     _id: object.steamid,
        //     //     inventory: {
        //     //         assets: object.assets,
        //     //         descriptions: object.descriptions,
        //     //         total_inventory_count: object.total_inventory_count
        //     //     },
        //     //     lastupdate: new Date().toISOString()
        //     // }

        //     const obj = this._createInventoryObject(object)

        //     const dbo = db.db('mydb')
        //     dbo.collection(this.collections.STEAM_USERS_INVENTORY)
        //         .insertOne(obj, (err, res) => {
        //             if (err) {
        //                 throw err
        //             }

        //             console.log(`[Database][${this.collections.STEAM_USERS_INVENTORY}] 1 user inventory inserted`)
        //             db.close()
        //     })
        // })
    }

    async findOneUser(steamid = null, vanityurl = null) {

        if (!steamid && !vanityurl) {
            throw new Error("Parameters [steamid] and [vanityurl] cannot be null")
        }

        let result = null
        let query = {}
        if (steamid) {
            query = { _id: steamid }
        } else {
            query = { vanityurl }
        }
        
        try {
            await this.client.connect()
            const db = await this.client.db('mydb')
            const col = await db.collection(this.collections.STEAM_USERS)
            result = await col.findOne(query)
        } catch (error) {
            throw error
        } finally {
            await this.client.close()
        }

        if (result) {
            result.steamid = result._id
            delete result._id
        }

        return result

        // this.client.connect(this.url, (err, db) => {
        //     if (err) {
        //         throw err
        //     }

        //     if (!steamid && !vanityurl) {
        //         throw new Error("Parameters [steamid] and [vanityurl] cannot be null")
        //     }

        //     let query = {}
        //     if (steamid) {
        //         query = { _id: steamid }
        //     } else {
        //         query = { vanityurl }
        //     }
            
        //     const dbo = db.db('mydb')
        //     dbo.collection(this.collections.STEAM_USERS)
        //         .findOne(query, function (err, result) {
        //             if (err) throw err
                    
        //             db.close()
        //             return result
        //     })
        // })
    }

    async findOneInventory(steamid) {

        if (!steamid) {
            throw new Error("Parameter [steamid] cannot be null")
        }

        let result = null
        const query = { _id: steamid }

        try {
            await this.client.connect()
            const db = await this.client.db('mydb')
            const col = await db.collection(this.collections.STEAM_USERS_INVENTORY)
            result = await col.findOne(query)
        } catch (error) {
            throw error
        } finally {
            await this.client.close()
        }

        return result

        // this.client.connect(this.url, (err, db) => {
        //     if (err) {
        //         throw err
        //     }

        //     if (!steamid) {
        //         throw new Error("Parameter [steamid] cannot be null")
        //     }

        //     const query = { _id: steamid }
            
        //     const dbo = db.db('mydb')
        //     dbo.collection(this.collections.STEAM_USERS_INVENTORY)
        //         .findOne(query, function (err, result) {
        //             if (err) throw err
                    
        //             db.close()
        //             return result
        //     })
        // })
    }

    async updateUser(object, steamid = null, vanityurl = null) {

        if (!steamid && !vanityurl) {
            throw new Error("Parameters [steamid] and [vanityurl] cannot be null")
        }

        let query = {}
        if (steamid) {
            query = { _id: steamid }
        } else {
            query = { vanityurl }
        }

        const newvalues = {
            $set: this._createUserObject(object)
        }
        
        try {
            await this.client.connect()
            const db = await this.client.db('mydb')
            const col = await db.collection(this.collections.STEAM_USERS)
            await col.updateOne(query, newvalues)
        } catch (error) {
            throw error
        } finally {
            await this.client.close()
        }

        // this.client.connect(this.url, (err, db) => {
        //     if (err) {
        //         throw err
        //     }

        //     if (!steamid && !vanityurl) {
        //         throw new Error("Parameters [steamid] and [vanityurl] cannot be null")
        //     }

        //     let query = {}
        //     if (steamid) {
        //         query = { _id: steamid }
        //     } else {
        //         query = { vanityurl }
        //     }

        //     const newvalues = {
        //         $set: this._createUserObject(object)
        //     }
            
        //     const dbo = db.db('mydb')
        //     dbo.collection(this.collections.STEAM_USERS)
        //         .updateOne(query, newvalues, function (err, res) {
        //             if (err) throw err
        //             db.close()
        //         })
        // })
    }

    async updateInventory(object, steamid) {

        if (!steamid) {
            throw new Error("Parameter [steamid] cannot be null")
        }

        const query = { _id: steamid }
        const newvalues = { $set: this._createInventoryObject(object) }

        try {
            await this.client.connect()
            const db = await this.client.db('mydb')
            const col = await db.collection(this.collections.STEAM_USERS_INVENTORY)
            await col.updateOne(query, newvalues)
        } catch (error) {
            throw error
        } finally {
            await this.client.close()
        }

        // this.client.connect(this.url, (err, db) => {
        //     if (err) {
        //         throw err
        //     }

        //     if (!steamid) {
        //         throw new Error("Parameter [steamid] cannot be null")
        //     }

        //     const query = { _id: steamid }
        //     const newvalues = { $set: this._createInventoryObject(object) }
            
        //     const dbo = db.db('mydb')
        //     dbo.collection(this.collections.STEAM_USERS_INVENTORY)
        //         .updateOne(query, newvalues, function (err, result) {
        //             if (err) throw err                    
        //             db.close()
        //     })
        // })
    }
}

module.exports = SteamMongoDatabase
