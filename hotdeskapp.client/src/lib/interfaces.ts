
export default interface ReservationsPublic{
    id: string,
    userId: string,
    deskId: string,
    deskName: string,
    locationId: string,
    locationName: string,
    startDate: string,
    endDate: string
}

export default interface UserPrivate{
    id: string,
    name: string,
    surname: string,
    email: string,
    lastTimeLogged: Date,
    userRole: number
}

export default interface LocationItem{
    id: string,
    name: string,
    description: string,
    coverImgPath: string
}

export default interface Desk{
    id:string,
    name: string,
    description: string,
    isMaintnance: boolean
}
