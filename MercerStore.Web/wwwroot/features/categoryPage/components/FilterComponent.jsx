import React, {useEffect, useState} from 'react';
import ReactSlider from 'react-slider';
import {useCategoryPriceRange} from '../hooks/useCategoryPriceRange';
import {useFetchCategoryPriceRange} from '../hooks/useFetchCategoryPriceRange';

const FilterComponent = () => {
    const {minPrice, maxPrice, selectedMinPrice, selectedMaxPrice} = useCategoryPriceRange();
    const {updateMinPrice, updateMaxPrice} = useFetchCategoryPriceRange();

    const [localMinPrice, setLocalMinPrice] = useState(selectedMinPrice);
    const [localMaxPrice, setLocalMaxPrice] = useState(selectedMaxPrice);

    useEffect(() => {
        if (minPrice !== undefined && maxPrice !== undefined) {
            setLocalMinPrice(selectedMinPrice);
            setLocalMaxPrice(selectedMaxPrice);
        }
    }, [selectedMinPrice, selectedMaxPrice]);

    const handleMinPriceInputChange = (e) => {
        const value = Number(e.target.value) || minPrice;
        setLocalMinPrice(value);
    };

    const handleMaxPriceInputChange = (e) => {
        const value = Number(e.target.value) || maxPrice;
        setLocalMaxPrice(value);
    };


    const handleSliderChange = (values) => {
        const [newMinPrice, newMaxPrice] = values;
        setLocalMinPrice(newMinPrice);
        setLocalMaxPrice(newMaxPrice);
    };


    const handleSliderAfterChange = (values) => {
        const [newMinPrice, newMaxPrice] = values;
        updateMinPrice(newMinPrice);
        updateMaxPrice(newMaxPrice);
    };

    return (
        <div className="filter-block">
            <h3>Фильтры</h3>
            <h4>Цена</h4>
            <div className="price-inputs">
                <div className="price-input-block">
                    <span>От</span>
                    <input
                        type="text"
                        className="price-input"
                        value={localMinPrice}
                        onChange={handleMinPriceInputChange}
                    />
                </div>
                <div className="price-input-block">
                    <span>До</span>
                    <input
                        type="text"
                        className="price-input"
                        value={localMaxPrice}
                        onChange={handleMaxPriceInputChange}
                    />
                </div>
            </div>
            <ReactSlider
                className="horizontal-slider"
                thumbClassName="thumb"
                trackClassName="track"
                min={minPrice}
                max={maxPrice}
                value={[localMinPrice, localMaxPrice]}
                onChange={handleSliderChange}
                onAfterChange={handleSliderAfterChange}
                ariaLabel={['Минимальная цена', 'Максимальная цена']}
                pearling
                minDistance={10}
            />
        </div>
    );
};

export default FilterComponent;