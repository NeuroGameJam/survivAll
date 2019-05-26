function [] = PlotHeatMap (tilesUsed, nTiles)
sideN = round(sqrt(nTiles));

tilesUsed = tilesUsed;

values2D = nan(numel(tilesUsed), 2);

for i = 1:size(tilesUsed,1)
    
    x_val(i) = rem(tilesUsed(i), sideN);
    
    if x_val(i) == 0
        x_val(i) = sideN;
    end
    
    y_val(i) = ceil(tilesUsed(i)/sideN);
end 

values2D = [x_val; y_val];

hist3(values2D', 'ctrs', {1:1:sideN 1:1:sideN});
xlabel('Tile number')
ylabel('Tile row')
title('Tile occupancy')
set(gcf, 'renderer', 'opengl')
set(get(gca,'child'),'FaceColor','interp','CDataMode','auto');
view(2)
colorbar

end 