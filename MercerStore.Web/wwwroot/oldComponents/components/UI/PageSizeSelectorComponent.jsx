import React from 'react';

const PageSizeSelectorComponent = ({pageSize, onChange}) => {
    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Показывать:</span>
            <select className="form-select" value={pageSize} onChange={onChange}>
                <option value="9">9</option>
                <option value="15">15</option>
                <option value="30">30</option>
                <option value="45">45</option>
            </select>
        </div>
    );
};

export default PageSizeSelectorComponent;