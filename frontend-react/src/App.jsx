import { React, useState } from 'react';
const API_URL = "https://localhost:7235";
export function App() {
    const [mascots, setMascots] = useState([]);
    async function handleLoad(event) {
        event.preventDefault();
        const response = await fetch(`${API_URL}mascots`);
        if (response.ok == true) {
            const json = await response.json();
            const newMascots = [];
            json.forEach(element => {
                newMascots.push(<li key={element.id}>Name:{element.name} | Species: {element.species}</li>);
            });
            setMascots(newMascots);
        }
    }
    return (
        <div className="container">
            <div className="row">
                <div className="col">
                    <h3>CoE .NET Prep</h3>
                    <button type="button" className="btn btn-primary" onClick={handleLoad}>Load Mascots</button>
                    <ul>
                        {mascots}
                    </ul>
                </div>
            </div>
        </div>
    );
}