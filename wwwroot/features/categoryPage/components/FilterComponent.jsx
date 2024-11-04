import React from 'react';
import ReactSlider from 'react-slider';
import { useCategoryPriceRange } from '../hooks/useCategoryPriceRange';

const FilterComponent = () => {
   
    const {
        minPrice,
        maxPrice,
        localMinPrice,
        localMaxPrice,
        setLocalMinPrice,
        setLocalMaxPrice,
        updatePriceRange
    } = useCategoryPriceRange();

    const handleMinPriceInputChange = (e) => {
        const value = Number(e.target.value) || minPrice;
        setLocalMinPrice(value);
        updatePriceRange(value, localMaxPrice);
    };

   
    const handleMaxPriceInputChange = (e) => {
        const value = Number(e.target.value) || maxPrice;
        setLocalMaxPrice(value);
        updatePriceRange(localMinPrice, value);
    };

    const handleSliderChange = (values) => {
        const [newMinPrice, newMaxPrice] = values;
        setLocalMinPrice(newMinPrice);
        setLocalMaxPrice(newMaxPrice);
        updatePriceRange(newMinPrice, newMaxPrice);
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
                ariaLabel={['Минимальная цена', 'Максимальная цена']}
                pearling
                minDistance={10}
            />
        </div>
    );
};

export default FilterComponent;