import React from 'react';
import { useDispatch } from 'react-redux';
import { useCategoryProducts } from '../hooks/useCategoryProducts';
import { setSortOrder } from '../redux/categorySlice';

const SortComponent = () => {
    const dispatch = useDispatch();
    const { sortOrder } = useCategoryProducts();

    const handleSortChange = (event) => {
        const newSortOrder = event.target.value;
        dispatch(setSortOrder(newSortOrder));  
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Сортировать:</span>
            <select className="form-select" value={sortOrder} onChange={handleSortChange}>
                <option value="">Имя (я-а)</option>
                <option value="name_desc">Имя (а-я)</option>
                <option value="price_asc">Сначала недорогие</option>
                <option value="price_desc">Сначала дорогие</option>
            </select>
        </div>
    );
};

export default SortComponent;