import React from 'react';

const SortComponent = ({ currentSort, onChange }) => {
    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Сортировать:</span>
            <select className="form-select" value={currentSort} onChange={onChange}>
                <option value="">Имя (я-а)</option>
                <option value="name_desc">Имя (а-я)</option>
                <option value="price_asc">Сначала недорогие</option>
                <option value="price_desc">Сначала дорогие</option>
            </select>
        </div>
    );
};

export default SortComponent;