import React, { Component } from 'react';
import Tree from 'react-d3-tree';

const myTreeData =
{
    name: 'Top Level Root',
    attributes: {
        keyA: 'val A',
        keyB: 'val B',
        keyC: 'val C',
    },
    children: [
        {
            name: 'Level 2: A',
            attributes: {
                keyA: 'val A',
                keyB: 'val B',
                keyC: 'val C',
            },
        },
        {
            name: 'Level 2: B',
        },
    ],
}

const svgSquare = {
    shape: 'rect',
    shapeProps: {
        width: 20,
        height: 20,
        x: -10,
        y: -10,
    }
}

const containerStyles = {
    width: '100%',
    height: '100vh',
}


export class Home extends Component {
    static displayName = Home.name;

    state = {
        treeJson: {},
        sourceCodeText: 'using System;\r\n\r\npublic class Program\r\n{\r\n    public static void Main(string[] args)\r\n    {\r\n        Console.WriteLine("Hello, world");\r\n    }\r\n}'
    };

    componentDidMount() {
        const dimensions = this.treeContainer.getBoundingClientRect();
        this.setState({
            translate: {
                x: dimensions.width / 2,
                y: dimensions.height / 2
            }
        });
    }

    handleChanged = () => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'text/plain;charset=UTF-8' },
            body: this.state.sourceCodeText
        };
        fetch('SyntaxTree', requestOptions)
            .then(response => response.json())
            .then(data => this.setState({ treeJson: data }));
    }

    render() {
        return (
            <React.Fragment>
                <textarea value={this.state.sourceCodeText} onChange={this.handleChanged} style={{ width: '100%' }} />
                <div id="treeWrapper" style={containerStyles} ref={tc => (this.treeContainer = tc)}>
                    <Tree data={this.state.treeJson} nodeSvgShape={svgSquare} orientation="vertical" translate={this.state.translate} />
                </div>
            </React.Fragment>
        );
    }
}
