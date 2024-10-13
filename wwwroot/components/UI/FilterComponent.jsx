import React from 'react';
import ReactSlider from 'react-slider';

const FilterComponent = ({ minPrice, maxPrice, selectedMinPrice, selectedMaxPrice, onMinPriceChange, onMaxPriceChange }) => {
    const handleSliderChange = (values) => {
        const [newMinPrice, newMaxPrice] = values;
        onMinPriceChange(newMinPrice);
        onMaxPriceChange(newMaxPrice);
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
                        value={selectedMinPrice}
                       
                    />
                </div>
                <div className="price-input-block">
                    <span>До</span>
                    <input
                        type="text"
                        className="price-input"
                        value={selectedMaxPrice}
                       
                    />
                </div>
            </div>
            <ReactSlider
                className="horizontal-slider"
                thumbClassName="thumb"
                trackClassName="track"
                min={minPrice} 
                max={maxPrice} 
                value={[selectedMinPrice, selectedMaxPrice]}
                onChange={handleSliderChange}
                ariaLabel={['Минимальная цена', 'Максимальная цена']}
                pearling
                minDistance={10}
            />
        </div>
    );
};

export default FilterComponent;