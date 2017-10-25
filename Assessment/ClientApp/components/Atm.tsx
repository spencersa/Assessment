import * as React from 'react';

interface AtmData {
    input: string;
    output: string[];
    loading: boolean;
}

export class Atm extends React.Component<{}, AtmData> {

    constructor() {
        super();
        this.state = { input: "", output: [], loading: true };

        fetch('api/Atm/GetWelcomeMessage')
            .then(response => response.json() as Promise<string[]>)
            .then(data => {
                this.setState({ output: data, loading: false });
            });
    }

    post = () => {
        fetch('api/Atm/SendCommand', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state.input.split(" "))
        })
            .then(response => response.json() as Promise<string[]>)
            .then(data => {
                this.setState({ output: data, input: "" });
            });
    }

    handleChange(event: any) {
        this.setState({ input: event.target.value })
    }

    render() {
        return <div className="row">
            <div className="col-xs-4"></div>
            <div className="col-xs-4 round-boarder">
                <table className="table table-striped">
                    <tbody>
                        {this.state.output.map(output =>
                            <tr key={output}>
                                <td>{output}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
                <form className="form-inline"
                    onKeyDown={
                        (e) => {
                            if (e.key === 'Enter') {
                                e.preventDefault();
                                this.post();
                            }
                        }
                    }
                    onSubmit={
                        (e) => {
                            e.preventDefault();
                            e.stopPropagation();
                            this.post();
                        }
                    }>
                    <input type="text"
                        className="form-control"
                        value={this.state.input}
                        onChange={this.handleChange.bind(this)}></input>
                    <button type="submit" className="btn btn-secondary">Send</button>
                </form>
            </div>
            <div className="col-xs-4"></div>
        </div>
    }
}