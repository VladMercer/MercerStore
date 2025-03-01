import React from 'react';
import { useDispatch } from 'react-redux';
import { useSearchPage } from '../hooks/useSearchPage';
import { setSortOrder } from '../redux/searchPageSlice';

const SortComponent = () => {
    const dispatch = useDispatch();
    const { sortOrder } = useSearchPage();

    const handleSortChange = (event) => {
        const newSortOrder = event.target.value;
        dispatch(setSortOrder(newSortOrder));  
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Сортировать:</span>
            <select className="form-select" value={sortOrder} onChange={handleSortChange}>
                <option value="name_asc">Имя (я-а)</option>
                <option value="name_desc">Имя (а-я)</option>
                <option value="price_asc">Сначала недорогие</option>
                <option value="price_desc">Сначала дорогие</option>
            </select>
        </div>
    );
};

export default SortComponent;