import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import { Atm } from './Atm'

export class Home extends React.Component<RouteComponentProps<{}>> {
    constructor() {
        super();
    }

    public render() {
        return <div>
            <br></br>
            <Atm></Atm>
        </div>;
    }
}