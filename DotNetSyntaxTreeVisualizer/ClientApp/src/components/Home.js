import React, { Component } from 'react';
import Tree from 'react-d3-tree';

const containerStyles = {
    width: '100%',
    height: '100vh',
}

const helloWorldCode = 'using System;\r\n\r\npublic class Program\r\n{\r\n    public static void Main(string[] args)\r\n    {\r\n        Console.WriteLine("Hello, world");\r\n    }\r\n}'

export class Home extends Component {
    static displayName = Home.name;

    state = {
        treeJson: {},
        sourceCodeText: helloWorldCode
    };

    componentDidMount() {
        const dimensions = this.treeContainer.getBoundingClientRect();
        this.setState({
            translate: {
                x: dimensions.width / 2,
                y: dimensions.height / 2
            }
        });
        this.handleChanged({
            target: {
                value: helloWorldCode
            }
        });
    }

    handleChanged = (event) => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'text/plain;charset=UTF-8' },
            body: event.target.value
        };
        fetch('SyntaxTree', requestOptions)
            .then(response => response.json())
            .then(data => this.setState({ treeJson: data }));
    }

    render() {
        return (
            <React.Fragment>
                <textarea defaultValue={helloWorldCode} onChange={this.handleChanged} style={{ height: '230px', width: '100%' }} />
                <div id="treeWrapper" style={containerStyles} ref={tc => (this.treeContainer = tc)}>
                    <Tree data={this.state.treeJson} orientation="vertical" translate={this.state.translate} pathFunc="straight" />
                </div>
            </React.Fragment>
        );
    }
}
